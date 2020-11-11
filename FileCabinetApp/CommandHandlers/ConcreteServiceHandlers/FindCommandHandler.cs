using System;
using System.Collections.Generic;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        public const string FindConstant = "find";
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
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

            if (string.Equals(FindConstant, commandRequest.Commands, StringComparison.OrdinalIgnoreCase))
            {
                this.Find(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Find(string parameters)
        {
            var findComandAttributes = parameters.Split(new char[] { ' ' });
            switch (findComandAttributes[0].ToUpper())
            {
                case "FIRSTNAME":
                    this.FindFirstName(findComandAttributes[1]);
                    break;
                case "LASTNAME":
                    this.FindLastName(findComandAttributes[1]);
                    break;
                case "DATEOFBIRTH":
                    this.FindDateOfBirth(findComandAttributes[1]);
                    break;
            }
        }

        private void FindFirstName(string firstName)
        {
            var temp = firstName.Substring(1, firstName.Length - 2);
            var findRecords = this.fileCabinetService.FindByFirstName(temp);

            this.printer(findRecords);
        }

        private void FindLastName(string lastName)
        {
            var temp = lastName.Substring(1, lastName.Length - 2);
            var findRecords = this.fileCabinetService.FindByLastName(temp);

            this.printer(findRecords);
        }

        private void FindDateOfBirth(string dateOfBirth)
        {
            var findRecords = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);

            this.printer(findRecords);
        }
    }
}
