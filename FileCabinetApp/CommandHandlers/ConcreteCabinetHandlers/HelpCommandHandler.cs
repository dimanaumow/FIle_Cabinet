using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Help command handler.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const string HelpConstant = "help";

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "show statistics by records.", "The 'create' command show statistics by records." },
            new string[] { "create", "receive user input and create new record.", "The 'exit' command receive user input and create new record." },
            new string[] { "export csv", "export records in csv format.", "The 'export csv' command export all records in csv format." },
            new string[] { "export xml", "export records in xml format.", "The 'export xml' command export all records in xml format." },
            new string[] { "import csv", "import records from csv file.", "The 'import csv' command import all records from csv file." },
            new string[] { "import xml", "import records from xml file.", "The 'import xml' command import all records from xml file." },
            new string[] { "delete", "deleted the records by given arguments or id.", "The 'delete' command deleted the records by given arguments or id." },
            new string[] { "insert", "edited the records with given arguments.", "The 'insert' command edited the records with given arguments." },
            new string[] { "update", "update the selected property for selected records.", "The 'update' command update the selected property for selected records." },
            new string[] { "select", "console selected records in the table format.", "The 'select' command console selected records in the table format" },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, HelpConstant, StringComparison.OrdinalIgnoreCase))
            {
                Help(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private static void Help(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
