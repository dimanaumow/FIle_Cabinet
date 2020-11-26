using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

#pragma warning disable CA1822

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Export handler.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private const string ExportConstant = "export";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

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

            if (string.Equals(ExportConstant, commandRequest.Commands, StringComparison.OrdinalIgnoreCase))
            {
                this.Export(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Export(string parameters)
        {
            var exportComandAttributes = parameters.Split(' ', 2);

            if (File.Exists(exportComandAttributes[1]))
            {
                if (!this.Rewrite($"File is exist - rewrite {exportComandAttributes[1]}?"))
                {
                    return;
                }
            }

            switch (exportComandAttributes[0].ToUpper(CultureInfo.InvariantCulture))
            {
                case "CSV":
                    this.ExportToCsv(exportComandAttributes[1]);
                    break;
                case "XML":
                    this.ExportToXml(exportComandAttributes[1]);
                    break;
                default:
                    Console.WriteLine("Your comand is incorrect.");
                    break;
            }
        }

        private void ExportToCsv(string fileName)
        {
            try
            {
                var snapshot = this.fileCabinetService.MakeSnapShot();
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

        private void ExportToXml(string fileName)
        {
            try
            {
                var snapshot = this.fileCabinetService.MakeSnapShot();
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

        private bool Rewrite(string question, bool defaultAnswer = true)
        {
            string yesOrNo = defaultAnswer ? "[Y/n] " : "[y/N] ";
            Console.Write($"{question} {yesOrNo}");

            do
            {
                var answer = Console.ReadLine().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(answer))
                {
                    return defaultAnswer;
                }
                else if (answer == "Y")
                {
                    return true;
                }
                else if (answer == "N")
                {
                    return false;
                }

                Console.Write(yesOrNo);
            }
            while (true);
        }
    }
}
