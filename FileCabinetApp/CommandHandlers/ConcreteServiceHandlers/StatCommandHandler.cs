using System;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public const string StatConstant = "stat";

        public StatCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, StatConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Stat(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.real} record(s).");
        }
    }
}
