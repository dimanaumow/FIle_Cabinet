using System;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public const string RemoveConstant = "remove";

        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, RemoveConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Remove(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Remove(string parameters)
        {
            var id = int.Parse(parameters);
            if (this.fileCabinetService.Remove(id))
            {
                Console.WriteLine($"Record #{id} was removed");
            }
            else
            {
                Console.WriteLine($"Record #{id} not founded or not exist.");
            }
        }
    }
}
