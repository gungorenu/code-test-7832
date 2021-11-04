using System;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("code-test-tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")] // sometimes I hate this InternalsVisibleTo
namespace CodeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IConsole console = new Console();
            try
            {
                string baseUri = console.ReadValue("Please enter full API url: ");
                IWebClient webClient = new WebClient(console, baseUri);

                // message loop: it shall start with Initialize (GenerateKey) and depending on the results shall ask for next available operation.
                // every operation is in a state and allows different other operations. system shall print them all and user can select one of the available options
                IJobApplication jobApp = new JobApplication(console, webClient);

                Operations nextSteps = jobApp.Initialize();
                if(!jobApp.IsInitialized)
                {
                    console.Log(ConsoleColor.Yellow, "Initialization is interrupted, cannot continue...\n");
                    return;
                }

                do
                {
                    try
                    {
                        // get next operation from console
                        Operations next = (Operations)console.ReadFlagsEnumValue("Please enter next step and press [Enter]: ", typeof(Operations), (int)nextSteps);

                        // maybe a better way of switch through all possible options?
                        // I have done something similar in one of my own applications and it works perfectly there so I applied same pattern
                        switch( next)
                        {
                            // these are not implemented yet
                            case Operations.AddAttachment:
                            case Operations.UpdateMetadata:
                            case Operations.UploadAttachment:
                                break;


                            // deletes the application info prepared until so far
                            case Operations.DeleteApplication:
                                nextSteps = jobApp.DeleteApplication();
                                break;

                            // updates the application info
                            case Operations.Update:
                                nextSteps = jobApp.Update();
                                break;

                            // registers the first application
                            case Operations.Register:
                                nextSteps = jobApp.Register();
                                break;

                            // finalizes the application as it is in final form
                            case Operations.MakeApplication:
                                nextSteps = jobApp.MakeApplication();
                                break;

                            // views current application info
                            case Operations.ViewApplication:
                                nextSteps = jobApp.ViewApplication();
                                break;

                            // this step is not allowed here, just giving error message
                            case Operations.GenerateKey:
                                console.Error("Generate key is not a valid options. Try again and report the bug to developer...\n");
                                continue;

                            // exit application
                            case Operations.Exit:
                                return;

                            // endless loop
                            case Operations.None:
                            default:
                                console.Clear();
                                continue;
                        }
                    }
                    catch (NotImplementedException)
                    {
                        console.Log(ConsoleColor.Yellow, "Operation not implemented, have patience...\n");
                    }
                    catch (Exception ex)
                    {
                        console.Log(ConsoleColor.Red, "Error occured during operation: {0}\n", ex.Message);
                        console.Log(ConsoleColor.Red, "StackTrace: {0}\n", ex.StackTrace);
                    }

                    // let user see the final result and continue the loop
                    console.Log(ConsoleColor.Gray, "Press any key to continue...\n");
                    console.ReadKey();
                    console.Clear();
                } while (true);
            }
            catch ( Exception ex)
            {
                console.Log(ConsoleColor.Red, "Error occured during initialization: {0}\n", ex.Message);
                console.Log(ConsoleColor.Red, "StackTrace: {0}\n", ex.StackTrace);
                console.Log(ConsoleColor.Red, "Application cannot continue, quitting...\n");
            }
            console.Log(ConsoleColor.Gray, "Application exiting, last chance to copy console. Press any key to quit...\n");
            console.ReadKey();
        }
    }
}
