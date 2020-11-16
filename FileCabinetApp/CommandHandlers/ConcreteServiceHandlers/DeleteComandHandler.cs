using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    public class DeleteComandHandler : ServiceCommandHandlerBase
    {
        public const string DeleteConstant = "delete";
        public const string DeleteKeyWord = "where";

        public DeleteComandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, DeleteConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Delete(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Delete(string parameters)
        {
            var (property, value) = this.Parse(parameters);

            if (string.Equals(property, "id", StringComparison.OrdinalIgnoreCase))
            {
                int id = int.Parse(value, CultureInfo.InvariantCulture);
                this.fileCabinetService.Remove(id);

                Console.WriteLine($"Record #{id} are deleted.");
            }

            var deletedRecords = this.fileCabinetService.FindBy(property, value);
            var sb = new StringBuilder();

            foreach (var record in deletedRecords)
            {
                sb.Append($"#{record.Id},");
                this.fileCabinetService.Remove(record.Id);
            }

            Console.WriteLine($"Records {sb} are deleted.");
        }

        private (string property, string value) Parse(string parameters)
        {
            if (!parameters.StartsWith(DeleteKeyWord, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{nameof(parameters)} must be start with {nameof(DeleteKeyWord)}");
            }

            parameters = parameters.Substring(DeleteKeyWord.Length);

            var deleteArray = parameters.Split(" = ");

            string property = deleteArray[0].Trim();
            string value = deleteArray[1].Trim('\'', ' ');

            return (property, value);
        }

        private string GeneRateName()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var sb = new StringBuilder();
            int countSymbols = random.Next(8, 60);

            for (int i = 0; i < countSymbols - 1; i++)
            {
                int position = random.Next(alphabet.Length - 1);
                sb.Append(i == 0 ? char.ToUpper(alphabet[position]) : alphabet[position]);
            }

            return sb.ToString();
        }

        private DateTime GenerateDateOfBirth()
        {
            var randomGenerator = new Random();
            DateTime startDate = new DateTime(1980, 1, 1);
            return startDate.AddDays(randomGenerator.Next((DateTime.Today - startDate).Days));
        }

        private char GenerateEnglishLevel()
        {
            string levels = "abc";
            var random = new Random();

            return levels[random.Next(0, 2)];
        }
    }
}
