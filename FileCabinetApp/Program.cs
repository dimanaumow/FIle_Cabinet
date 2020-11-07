using System;
using System.Globalization;
using System.IO;
using System.Text;
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
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static IFileCabinetService fileCabinetService;
        private static IRecordValidator validator;
        private static bool isDefaultRule;
        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "show statistics by records.", "The 'create' command show statistics by records." },
            new string[] { "create", "receive user input and create new record.", "The 'exit' command receive user input and create new record." },
            new string[] { "list", "return a list of records added to the service.", "The 'exit' command return a list of records added to the service." },
            new string[] { "edit", "edit record", "The 'edit' comand edit record" },
            new string[] { "find firstName", "return a list of records with desired firstName.", "The 'find firstName' comand return a list of records with finded firstName." },
            new string[] { "find lastName", "return a list of records with desired lastName.", "The 'find lastName' comand return a list of records with finded lastName." },
            new string[] { "find dateofbirth", "return a list of records with desired date of birth.", "The 'find dateOfBirth' comand return a list of records with finded date of birth." },
            new string[] { "export csv", "export records in csv format.", "The 'export csv' command export all records in csv format." },
            new string[] { "export xml", "export records in xml format.", "The 'export xml' command export all records in xml format." },
            new string[] { "import csv", "import records from csv file.", "The 'import csv' command import all records from csv file." },
            new string[] { "import xml", "import records from xml file.", "The 'import xml' command import all records from xml file." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        private static string[] comandLineParameters = new string[]
        {
            "--validation-rules",
            "-v",
            "--storage",
            "-s",
        };

        #region Converters_and_validators
        private static Func<string, Tuple<bool, string, string>> stringConvrter = input =>
        {
            return new Tuple<bool, string, string>(true, input, input);
        };

        private static Func<string, Tuple<bool, string, DateTime>> dateConvrter = input =>
        {
            DateTime date;
            bool isValid = DateTime.TryParse(input, new CultureInfo("en-US"), DateTimeStyles.None, out date);

            return new Tuple<bool, string, DateTime>(isValid, input, date);
        };

        private static Func<string, Tuple<bool, string, short>> expirienceConverter = input =>
        {
            short expirience;
            bool isValid = short.TryParse(input, out expirience);

            return new Tuple<bool, string, short>(isValid, input, expirience);
        };

        private static Func<string, Tuple<bool, string, decimal>> balanceConverter = input =>
        {
            decimal balance;
            bool isValid = decimal.TryParse(input, out balance);

            return new Tuple<bool, string, decimal>(isValid, input, balance);
        };

        private static Func<string, Tuple<bool, string, char>> nationalityConverter = input =>
        {
            char nationality;
            bool isValid = char.TryParse(input, out nationality);

            return new Tuple<bool, string, char>(isValid, input, nationality);
        };

        private static Func<string, Tuple<bool, string>> firstNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        private static Func<string, Tuple<bool, string>> lastNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        private static Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = date =>
        {
            bool isValid = !(date < new DateTime(1950, 1, 1) || date > DateTime.Now);
            return new Tuple<bool, string>(isValid, date.ToString());
        };

        private static Func<short, Tuple<bool, string>> expirienceValidator = input =>
        {
            bool isValid = input >= 0 || isDefaultRule;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        private static Func<decimal, Tuple<bool, string>> balanceValidator = input =>
        {
            bool isValid = input >= 0 || isDefaultRule;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        private static Func<char, Tuple<bool, string>> nationalityValidator = nationality =>
        {
            bool isValid = char.IsLetter(nationality) || isDefaultRule;
            return new Tuple<bool, string>(isValid, nationality.ToString());
        };
        #endregion

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

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
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
                    validator = new DefaultValidator();
                    break;
                case "CUSTOM":
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                    Console.WriteLine("Using custom validation rules.");
                    validator = new CustomValidator();
                    break;
                default:
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    isDefaultRule = true;
                    validator = new DefaultValidator();
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

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = ReadInput(stringConvrter, firstNameValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(stringConvrter, lastNameValidator);

            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(dateConvrter, dateOfBirthValidator);

            Console.Write("Nationality: ");
            var nationality = ReadInput(nationalityConverter, nationalityValidator);

            Console.Write("Expirience: ");
            var expirience = ReadInput(expirienceConverter, expirienceValidator);

            Console.Write("Balance: ");
            var balance = ReadInput(balanceConverter, balanceValidator);

            var index = fileCabinetService.CreateRecord(new RecordData(firstName, lastName, dateOfBirth, expirience, balance, nationality));
            Console.WriteLine($"Record #{index} is created.");
        }

        private static void Edit(string parameters)
        {
            var id = int.Parse(parameters);
            if (id > fileCabinetService.GetStat())
            {
                Console.WriteLine($"#{id} records is not found.");
            }
            else
            {
                Console.Write("First name: ");
                var firstName = ReadInput(stringConvrter, firstNameValidator);

                Console.Write("Last name: ");
                var lastName = ReadInput(stringConvrter, lastNameValidator);

                Console.Write("Date of birth: ");
                var dateOfBirth = ReadInput(dateConvrter, dateOfBirthValidator);

                Console.Write("Nationality: ");
                var nationality = ReadInput(nationalityConverter, nationalityValidator);

                Console.Write("Expirience: ");
                var expirience = ReadInput(expirienceConverter, expirienceValidator);

                Console.Write("Balance: ");
                var balance = ReadInput(balanceConverter, balanceValidator);

                fileCabinetService.EditRecord(id, new RecordData(firstName, lastName, dateOfBirth, expirience, balance, nationality));
                Console.WriteLine($"Record #{id} is edited.");
            }
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}: {record.FirstName} {record.LastName}; Date of birth: {record.DateOfBirth.ToLongDateString()}" +
                    $" Expirience: {record.Expirience} years, Balance: {record.Balance}, Nationality: {record.Nationality}.");
            }
        }

        private static void Find(string parametrs)
        {
            var findComandAttributes = parametrs.Split(new char[] { ' ' });
            switch (findComandAttributes[0].ToUpper())
            {
                case "FIRSTNAME":
                    FindFirstName(findComandAttributes[1]);
                    break;
                case "LASTNAME":
                    FindLastName(findComandAttributes[1]);
                    break;
                case "DATEOFBIRTH":
                    FindDateOfBirth(findComandAttributes[1]);
                    break;
            }
        }

        private static void FindFirstName(string firstName)
        {
            var temp = firstName.Substring(1, firstName.Length - 2);
            var findRecords = fileCabinetService.FindByFirstName(temp);

            foreach (var record in findRecords)
            {
                Console.WriteLine($"#{record.Id}: {record.FirstName} {record.LastName}; Date of birth: {record.DateOfBirth.ToLongDateString()}" +
                    $" Expirience: {record.Expirience} years, Balance: {record.Balance}, Nationality: {record.Nationality}.");
            }
        }

        private static void FindLastName(string lastName)
        {
            var temp = lastName.Substring(1, lastName.Length - 2);
            var findRecords = fileCabinetService.FindByLastName(temp);

            foreach (var record in findRecords)
            {
                Console.WriteLine($"#{record.Id}: {record.FirstName} {record.LastName}; Date of birth: {record.DateOfBirth.ToLongDateString()}" +
                    $" Expirience: {record.Expirience} years, Balance: {record.Balance}, Nationality: {record.Nationality}.");
            }
        }

        private static void FindDateOfBirth(string dateOfBirth)
        {
            var findRecords = fileCabinetService.FindByDateOfBirth(dateOfBirth);

            foreach (var record in findRecords)
            {
                Console.WriteLine($"#{record.Id}: {record.FirstName} {record.LastName}; Date of birth: {record.DateOfBirth.ToLongDateString()}" +
                    $" Expirience: {record.Expirience} years, Balance: {record.Balance}, Nationality: {record.Nationality}.");
            }
        }

        private static void Export(string parameters)
        {
            var exportComandAttributes = parameters.Split(' ', 2);

            switch (exportComandAttributes[0].ToUpper())
            {
                case "CSV":
                    ExportToCsv(exportComandAttributes[1]);
                    break;
                case "XML":
                    ExportToXml(exportComandAttributes[1]);
                    break;
                default:
                    Console.WriteLine("Your comand is incorrect.");
                    break;
            }
        }

        private static void ExportToCsv(string fileName)
        {
            try
            {
                var snapshot = fileCabinetService.MakeSnapShot();
                using (var streamWriter = new StreamWriter(fileName, false))
                {
                    snapshot.SaveToCSV(streamWriter);
                    Console.WriteLine($"All record write in file {fileName}");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot be open this file {fileName}");
            }
        }

        private static void ExportToXml(string fileName)
        {
            try
            {
                var snapshot = fileCabinetService.MakeSnapShot();
                using (var streamWriter = new StreamWriter(fileName, false))
                {
                    snapshot.SaveToXml(streamWriter);
                    Console.WriteLine($"All record write in file {fileName}");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot be open this file {fileName}");
            }
        }

        private static void Import(string args)
        {
            var importCommandAttributes = args.Split(' ', 2);

            switch (importCommandAttributes[0].ToUpper())
            {
                case "CSV":
                    ImportCsv(importCommandAttributes[1]);
                    break;
                case "XML":
                    ImportXml(importCommandAttributes[1]);
                    break;
                default:
                    Console.WriteLine("Import error: invalid import file type");
                    break;
            }
        }

        private static void ImportCsv(string path)
        {
            var snapshot = new FileCabinetServiceSnapshot(Array.Empty<FileCabinetRecord>());
            try
            {
                using (var stream = File.Open(@path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var st = new StreamReader(stream))
                {
                    snapshot.LoadFromCsv(st);
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Import error: file {path} not exist");
            }

            int completed = fileCabinetService.Restore(snapshot);
            Console.WriteLine($"{completed} recordses were imported from {path}");
        }

        private static void ImportXml(string path)
        {

        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
