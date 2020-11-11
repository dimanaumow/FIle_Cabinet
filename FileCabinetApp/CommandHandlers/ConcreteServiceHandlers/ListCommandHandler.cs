using System;
using System.Collections.Generic;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public const string ListConstant = "list";
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        public ListCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(fileCabinetService)
        {
            if (printer is null)
            {
                throw new ArgumentNullException($"{nameof(printer)} cannot be null.");
            }

            this.printer = printer;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, ListConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.List(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void List(string parameters)
        {
            var records = this.fileCabinetService.GetRecords();

            this.printer(records);
        }
    }
}
