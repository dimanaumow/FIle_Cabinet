using System;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public const string CreateConstant = "create";

        public CreateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, CreateConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Create(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        public void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = InputValidator.ReadInput(InputValidator.stringConvrter, InputValidator.firstNameValidator);

            Console.Write("Last name: ");
            var lastName = InputValidator.ReadInput(InputValidator.stringConvrter, InputValidator.lastNameValidator);

            Console.Write("Date of birth: ");
            var dateOfBirth = InputValidator.ReadInput(InputValidator.dateConvrter, InputValidator.dateOfBirthValidator);

            Console.Write("EnglishLevel: ");
            var englishLevel = InputValidator.ReadInput(InputValidator.englishLevelConverter, InputValidator.englishLevelValidator);

            Console.Write("Experience: ");
            var experience = InputValidator.ReadInput(InputValidator.experienceConverter, InputValidator.experienceValidator);

            Console.Write("Balance: ");
            var balance = InputValidator.ReadInput(InputValidator.balanceConverter, InputValidator.balanceValidator);

            var index = this.fileCabinetService.CreateRecord(new RecordData(firstName, lastName, dateOfBirth, experience, balance, englishLevel));
            Console.WriteLine($"Record #{index} is created.");
        }
    }
}
