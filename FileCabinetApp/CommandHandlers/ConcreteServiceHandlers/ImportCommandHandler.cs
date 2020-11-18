using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    /// <summary>
    /// Import handler.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private const string ImportConstant = "import";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(commandRequest.Commands, ImportConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Import(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Import(string parameters)
        {
            var importCommandAttributes = parameters.Split(' ', 2);

            switch (importCommandAttributes[0].ToUpper(CultureInfo.InvariantCulture))
            {
                case "CSV":
                    this.ImportCsv(importCommandAttributes[1]);
                    break;
                case "XML":
                    this.ImportXml(importCommandAttributes[1]);
                    break;
                default:
                    Console.WriteLine("Import error: invalid import file type");
                    break;
            }
        }

        private void ImportCsv(string path)
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
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Import error: file {path} not exist");
            }

            int completed = this.fileCabinetService.Restore(snapshot);
            Console.WriteLine($"{completed} recordses were imported from {path}");
        }

        private void ImportXml(string path)
        {
            var snapshot = new FileCabinetServiceSnapshot(Array.Empty<FileCabinetRecord>());
            try
            {
                using (var stream = File.Open(@path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var st = new StreamReader(stream))
                {
                    snapshot.LoadFromXml(st);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Import error: file {path} not exist");
            }

            int completed = this.fileCabinetService.Restore(snapshot);
            Console.WriteLine($"{completed} recordses were imported from {path}");
        }
    }
}
