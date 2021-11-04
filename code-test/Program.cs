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
                IJobApplication jobApp = new JobApplication(console, webClient);

                // message loop is in the function
                jobApp.Run();
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
