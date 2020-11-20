using System;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create command handler.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private const string CreateConstant = "create";

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(commandRequest.Commands, CreateConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Create();
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Create()
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

            try
            {
                var index = this.fileCabinetService.CreateRecord(new RecordData { FirstName = firstName, LastName = lastName, Balance = balance, DateOfBirth = dateOfBirth, EnglishLevel = englishLevel, Experience = experience });
                Console.WriteLine($"Record #{index} is created.");
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Incorrect input: {exception.Message}");
            }
        }
    }
}
