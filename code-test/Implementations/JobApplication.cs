using System;

namespace CodeTest
{
    public class JobApplication : IJobApplication, IJobApplicationInternal
    {
        #region Fields
        private readonly IConsole _console;
        private readonly IWebClient _client;
        private string _password;
        private string _appKey;
        private string _userKey;
        #endregion

        #region Key Properties
        private bool IsRegistered => !string.IsNullOrEmpty(_appKey);
        public bool IsInitialized => !string.IsNullOrEmpty(_userKey);

        private Operations NotRegistered => Operations.None | Operations.Exit | Operations.Register;
        private Operations NotApplied => Operations.None | Operations.Exit | Operations.AddAttachment |
            Operations.DeleteApplication | Operations.MakeApplication | Operations.Update |
            Operations.UpdateMetadata | Operations.UploadAttachment | Operations.ViewApplication;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        internal JobApplication(IConsole console, IWebClient client)
        {
            if (console == null)
                throw new ArgumentNullException(nameof(console));
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _console = console;
            _client = client;
        }

        #region Members
        public Operations DeleteApplication()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to delete");

            var model = ModelHelper.ApplicationBaseModel(_appKey, _password);

            var request = _client.DELETE("/api/v1/application/delete");
            request.AddHeader("X-Recruitment-Api-Key", _userKey);
            request.AddJsonBodyParameter(model);

            var response = request.Execute<ApiResponseObject>();
            HandleResponse(response);
            
            if(response.Status)
            {
                _appKey = null;
                _password = null;
                return NotRegistered;
            }
            else return NotApplied;
        }

        public Operations MakeApplication()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to apply");

            var model = ModelHelper.ApplicationBaseModel(_appKey, _password);

            var request = _client.POST("/api/v1/application/apply");
            request.AddHeader("X-Recruitment-Api-Key", _userKey);
            request.AddJsonBodyParameter(model);
            var response = request.Execute<ApiResponseObject>();
            HandleResponse(response);

            if (response.Status)
            {
                _appKey = null;
                return NotRegistered;
            }
            else return NotApplied;
        }

        public Operations Register()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Initialization is not completed");

            var model = _console.InputObject(new { email = "", name = "", password = "", phone = "" });

            var request = _client.POST("/api/v1/application/register");
            request.AddHeader("X-Recruitment-Api-Key", _userKey);
            request.AddJsonBodyParameter(model);
            var response = request.Execute<AppKeyApiResponseObject>();
            HandleResponse(response);
            _appKey = response.ApplicationKey;
            // we require password
            _password = model["password"].ToString();

            if (response.Status)
                return NotApplied;
            else
                return NotRegistered;
        }

        public Operations Initialize()
        {
            if (IsInitialized)
                throw new InvalidOperationException("Already initialized the application. Re-initialization is not necessary");
            if (IsRegistered)
                throw new InvalidOperationException("Another application is registered, delete or finalize the application first");

            var model = _console.InputObject(new { userEmail = "", userPassword = "" });

            var request = _client.POST("/api/v1/key/generate");
            request.AddJsonBodyParameter(model);
            var response = request.Execute<UserKeyApiResponseObject>();
            HandleResponse(response);
            if( response.Status)
                _userKey = response.UserKey;

            return NotRegistered;
        }

        public Operations Update()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to update");

            throw new NotImplementedException();

            return NotApplied;
        }

        public Operations UpdateMetadata()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to update");

            throw new NotImplementedException();

            return NotApplied;
        }

        public Operations AddAttachment()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to update");

            throw new NotImplementedException();

            return NotApplied;
        }

        public Operations UploadAttachment()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to update");

            throw new NotImplementedException();

            return NotApplied;
        }

        public Operations ViewApplication()
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Application has not been registered, nothing to show");

            var request = _client.GET("/api/v1/application/view"); // ?applicationKey={applicationKey}&password={password}
            request.AddHeader("X-Recruitment-Api-Key", _userKey);
            request.AddParameter("applicationKey", _appKey);
            request.AddParameter("password", _password);
            string responseContent = string.Empty;
            var response = request.Execute<ApiResponseObject>(out responseContent);
            HandleResponse(response);
            if (response.Status)
            {
                var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseContent);
                _console.Print(responseJson);
            }

            return NotApplied;
        }

        public void Run()
        {
            if (IsInitialized)
                throw new InvalidOperationException("Already initialized the application. Re-initialization is not necessary");

            // it shall start with Initialize (GenerateKey) and depending on the results shall ask for next available operation.
            Operations nextSteps = Initialize();
            if (!IsInitialized)
            {
                _console.Log(ConsoleColor.Yellow, "Initialization is interrupted, cannot continue...\n");
                return;
            }

            // message loop: every operation is in a state and allows different other operations. system shall print them all and user can select one of the available options
            do
            {
                try
                {
                    // get next operation from console
                    Operations next = (Operations)_console.ReadFlagsEnumValue("Please enter next step and press [Enter]: ", typeof(Operations), (int)nextSteps);

                    // maybe a better way of switch through all possible options?
                    // I have done something similar in one of my own applications and it works perfectly there so I applied same pattern
                    switch (next)
                    {
                        // ads new attachment
                        case Operations.AddAttachment:
                            nextSteps = AddAttachment();
                            break;

                        // updates metadata
                        case Operations.UpdateMetadata:
                            nextSteps = UpdateMetadata();
                            break;

                        // uploads new attachment
                        case Operations.UploadAttachment:
                            nextSteps = UploadAttachment();
                            break;

                        // deletes the application info prepared until so far
                        case Operations.DeleteApplication:
                            nextSteps = DeleteApplication();
                            break;

                        // updates the application info
                        case Operations.Update:
                            nextSteps = Update();
                            break;

                        // registers the first application
                        case Operations.Register:
                            nextSteps = Register();
                            break;

                        // finalizes the application as it is in final form
                        case Operations.MakeApplication:
                            nextSteps = MakeApplication();
                            break;

                        // views current application info
                        case Operations.ViewApplication:
                            nextSteps = ViewApplication();
                            break;

                        // this step is not allowed here, just giving error message
                        case Operations.GenerateKey:
                            _console.Error("Generate key is not a valid options. Try again and report the bug to developer...\n");
                            continue;

                        // exit application
                        case Operations.Exit:
                            return;

                        // endless loop
                        case Operations.None:
                        default:
                            _console.Clear();
                            continue;
                    }
                }
                catch (NotImplementedException)
                {
                    _console.Log(ConsoleColor.Yellow, "Operation not implemented, have patience...\n");
                }
                catch (Exception ex)
                {
                    _console.Log(ConsoleColor.Red, "Error occured during operation: {0}\n", ex.Message);
                    _console.Log(ConsoleColor.Red, "StackTrace: {0}\n", ex.StackTrace);
                }

                // let user see the final result and continue the loop
                _console.Log(ConsoleColor.Gray, "Press any key to continue...\n");
                _console.ReadKey();
                _console.Clear();
            } while (true);
        }

        #endregion

        #region Internal Members
        internal void HandleResponse(ApiResponseObject responseObject)
        {
            if (responseObject.Status)
                _console.Log(ConsoleColor.Green, "Success: {0}\n", responseObject.Message);
            else
                _console.Log(ConsoleColor.Red, "Failure: {0}\n", responseObject.Message);
        }
        #endregion

        #region IJobApplicationInternal Members

        string IJobApplicationInternal.UserKey => _userKey;
        string IJobApplicationInternal.AppKey => _appKey;
        #endregion
    }
}
