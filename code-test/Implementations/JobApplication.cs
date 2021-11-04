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

            _appKey = null;

            return NotRegistered;
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
            _appKey = null;

            return NotRegistered;
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

            return NotApplied;
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

            var request = _client.GET("/api/v1/application/view?applicationKey={applicationKey}&password={password}");
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
