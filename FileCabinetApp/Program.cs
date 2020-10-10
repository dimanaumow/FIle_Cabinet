using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Dzmitry Naumov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "show statistics by records.", "The 'create' command show statistics by records." },
            new string[] { "create", "receive user input and create new record.", "The 'exit' command receive user input and create new record." },
            new string[] { "list", "return a list of records added to the service.", "The 'exit' command return a list of records added to the service." },
            new string[] {"edit", "edit record", "The 'edit' comand edit record"},
            new string[] { "find", "return a list of records with desired property.", "The 'find' comand return a list of records with finded property." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            var firstName = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                Console.Write("Incorrect input. Enter again first name: ");
                firstName = Console.ReadLine();
            }

            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                Console.Write("Incorrect input. Enter again last name: ");
                lastName = Console.ReadLine();
            }

            Console.Write("Date of birth: ");
            var dataOfBirth = Console.ReadLine();
            DateTime date;
            CultureInfo iOCultureFormat = new CultureInfo("en-US");
            DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
            while (date < new DateTime(1950, 1, 1) || date > DateTime.Now)
            {
                Console.Write("Incorrect input. Enter again date of birth: ");
                dataOfBirth = Console.ReadLine();
                DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
            }

            Console.Write("Nationality: ");
            var nationality = char.Parse(Console.ReadLine());

            Console.Write("Expirience: ");
            var expirience = short.Parse(Console.ReadLine());
            while (expirience < 0 || expirience > DateTime.Now.Year - date.Year)
            {
                Console.Write("Incorrect input. Enter again expirience: ");
                expirience = short.Parse(Console.ReadLine());
            }

            Console.Write("Balance: ");
            var balance = decimal.Parse(Console.ReadLine());

            var index = fileCabinetService.CreateRecord(firstName, lastName, date, expirience, balance, nationality);
            Console.WriteLine($"Record #{index} is created.");
        }

        public static void Edit(string parameters)
        {
            Console.Write("Input id recodrds for editing: ");
            var id = int.Parse(Console.ReadLine());
            if (id > fileCabinetService.GetStat())
            {
                Console.WriteLine($"#{id} records is not found.");
            }
            else
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
                {
                    Console.Write("Incorrect input. Enter again first name: ");
                    firstName = Console.ReadLine();
                }

                Console.Write("Last name: ");
                var lastName = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
                {
                    Console.Write("Incorrect input. Enter again last name: ");
                    lastName = Console.ReadLine();
                }

                Console.Write("Date of birth: ");
                var dataOfBirth = Console.ReadLine();
                DateTime date;
                CultureInfo iOCultureFormat = new CultureInfo("en-US");
                DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
                while (date < new DateTime(1950, 1, 1) || date > DateTime.Now)
                {
                    Console.Write("Incorrect input. Enter again date of birth: ");
                    dataOfBirth = Console.ReadLine();
                    DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
                }

                Console.Write("Nationality: ");
                var nationality = char.Parse(Console.ReadLine());

                Console.Write("Expirience: ");
                var expirience = short.Parse(Console.ReadLine());
                while (expirience < 0 || expirience > DateTime.Now.Year - date.Year)
                {
                    Console.Write("Incorrect input. Enter again expirience: ");
                    expirience = short.Parse(Console.ReadLine());
                }

                Console.Write("Balance: ");
                var balance = decimal.Parse(Console.ReadLine());

                fileCabinetService.EditRecord(id, firstName, lastName, date, expirience, balance, nationality);
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
    }
}
