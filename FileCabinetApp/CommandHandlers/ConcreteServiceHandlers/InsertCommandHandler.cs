using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Memoization;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        public const string InsertConstant = "insert";
        private const string InsertKeyWord = "values";

        public InsertCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, InsertConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Insert(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Insert(string parameters)
        {
            var (properties, values) = this.Parse(parameters);
            var recordPropertyInfo = typeof(FileCabinetRecord).GetProperties();

            var record = new FileCabinetRecord()
            {
                FirstName = this.GeneRateName(),
                LastName = this.GeneRateName(),
                DateOfBirth = this.GenerateDateOfBirth(),
                EnglishLevel = this.GenerateEnglishLevel(),
            };

            for (int i = 0; i < properties.Length; i++)
            {
                var recordProperty = recordPropertyInfo.FirstOrDefault(prop => string.Equals(prop.Name, properties[i], StringComparison.OrdinalIgnoreCase));
                if (recordProperty is null)
                {
                    continue;
                }

                var converter = TypeDescriptor.GetConverter(recordProperty.PropertyType);
                recordProperty.SetValue(record, converter.ConvertFromInvariantString(values[i]));
            }

            var data = new RecordData()
            {
                firstName = record.FirstName, //is null ? this.GeneRateName() : record.FirstName,
                lastName = record.LastName, //is null ? this.GeneRateName() : record.LastName,
                dateOfBirth = record.DateOfBirth,
                experience = record.Experience,
                balance = record.Balance,
                englishLevel = record.EnglishLevel,
            };

            this.fileCabinetService.CreateRecord(data);
            CashedData.ClearCashe();
        }

        private (string[] properties, string[] values) Parse(string parameters)
        {
            var insertArray = parameters.Split(InsertKeyWord);

            if (insertArray.Length != 2)
            {
                throw new ArgumentException($"{InsertConstant} input incorrect.");
            }

            var properties = insertArray[0].Split('(', ')', ',', ' ');
            var values = insertArray[1].Split('(', ')', ',', ' ', '\'');

            values = values.Where(x => x.Length != 0).ToArray();
            properties = properties.Where(x => x.Length != 0).ToArray();

            return (properties, values);
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
