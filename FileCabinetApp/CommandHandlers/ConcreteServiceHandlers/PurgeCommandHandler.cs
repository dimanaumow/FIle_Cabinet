using System;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public const string PurgeConstant = "purge";

        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, PurgeConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Purge(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Purge(string parameters)
        {
            this.fileCabinetService.Purge();
            Console.WriteLine("Data file processing is completed");
        }
    }
}
