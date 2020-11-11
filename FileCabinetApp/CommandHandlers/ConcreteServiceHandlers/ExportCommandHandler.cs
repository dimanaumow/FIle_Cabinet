using System;
using System.IO;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        public const string ExportConstant = "export";

        public ExportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

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

            switch (exportComandAttributes[0].ToUpper())
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
    }
}
