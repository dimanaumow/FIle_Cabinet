using System;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public const string ListConstant = "list";

        public ListCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}: {record.FirstName} {record.LastName}; Date of birth: {record.DateOfBirth.ToLongDateString()}" +
                    $" Expirience: {record.Expirience} years, Balance: {record.Balance}, Nationality: {record.Nationality}.");
            }
        }
    }
}
