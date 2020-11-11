using System;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.ConcreteServiceHandlers;
using FileCabinetApp.Printers;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Provide work with file cabinet service.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Dzmitry Naumov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static IFileCabinetService fileCabinetService;
        private static IRecordValidator validator;
        private static bool isDefaultRule;
        private static bool isRunning = true;

        private static string[] comandLineParameters = new string[]
        {
            "--validation-rules",
            "-v",
            "--storage",
            "-s",
        };

        /// <summary>
        /// Start point for application.
        /// </summary>
        /// <param name="args">Array of command-line keys passed to the program.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            CommandAgrsHandler(args);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                var commandHandlers = CreateCommandHandlers();

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                const int parametersIndex = 1;
                string parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandlers.Handle(new AppCommandRequest(command, parameters));
                Console.WriteLine();
            }
            while (isRunning);
        }

        private static void CommandAgrsHandler(string[] args)
        {
            string rule;
            int commandIndex = ParseRule(args, out rule);
            switch (rule)
            {
                case "DEFAULT":
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    isDefaultRule = true;
                    validator = new ValidatorBuilder().CreateDefault();
                    break;
                case "CUSTOM":
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                    Console.WriteLine("Using custom validation rules.");
                    validator = new ValidatorBuilder().CreateCustom();
                    break;
                default:
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    isDefaultRule = true;
                    validator = new ValidatorBuilder().CreateDefault();
                    break;
            }

            if (commandIndex >= 3)
            {
                switch (rule)
                {
                    case "MEMORY":
                        fileCabinetService = new FileCabinetMemoryService(validator);
                        Console.WriteLine("Use memory service");
                        break;
                    case "FILE":
                        string fullPath = "cabinet-records.db";
                        FileStream fileStream = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream);
                        Console.WriteLine("Use file service");
                        break;
                }
            }
        }

        private static int ParseRule(string[] args, out string rule)
        {
            if (args.Length == 0)
            {
                rule = string.Empty;
                return -1;
            }

            int index = -1;

            var parseText = args[0].Split(' ');
            if (parseText[0] == comandLineParameters[0])
            {
                rule = parseText[parseText.Length - 1].ToUpper();
                index = 0;
            }
            else if (parseText[0] == comandLineParameters[1])
            {
                rule = args[1].ToUpper();
                index = 1;
            }
            else if (parseText[0] == comandLineParameters[2])
            {
                rule = parseText[parseText.Length - 1].ToUpper();
                index = 3;
            }
            else if (parseText[0] == comandLineParameters[3])
            {
                rule = args[1].ToUpper();
                index = 4;
            }
            else
            {
                rule = string.Empty;
            }

            return index;
        }

        public static ICommandHandler CreateCommandHandlers()
        {
            static void Runner(bool x) => isRunning = x;

            var printer = new SimplePrinter();
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(Runner);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, printer.Print);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, printer.Print);
            var helpHandler = new HelpCommandHandler();
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var editHandler = new EditCommandHandler(fileCabinetService);

            createHandler.SetNext(exitHandler);
            exitHandler.SetNext(exportHandler);
            exportHandler.SetNext(findHandler);
            findHandler.SetNext(importHandler);
            importHandler.SetNext(listHandler);
            listHandler.SetNext(helpHandler);
            helpHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(statHandler);
            statHandler.SetNext(removeHandler);
            removeHandler.SetNext(editHandler);

            return createHandler;
        }
    }
}
