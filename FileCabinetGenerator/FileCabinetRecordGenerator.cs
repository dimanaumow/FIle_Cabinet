using System;
using System.Globalization;
using System.Text;
using FileCabinetApp.Configurations;
using FileCabinetApp.Service;

namespace FileCabinetGenerator
{
    public class FileCabinetRecordGenerator
    {
        public const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly Random randomGenerator;
        private readonly ConfigurationSetter setter;


        public FileCabinetRecordGenerator()
        {
            this.setter = new ConfigurationSetter("custom");
            this.randomGenerator = new Random();
        }

        public FileCabinetRecord Generate(int recordId)
        {
            var validParameters = this.setter.GetParameters();
            var record = new FileCabinetRecord();

            record.Id = recordId;
            record.FirstName = this.GenerateName(validParameters.FirstNameMinLength, validParameters.FirstNameMaxLenght);
            record.LastName = this.GenerateName(validParameters.LastNameMinLength, validParameters.LastNameMaxLength);
            record.DateOfBirth = GenerateDateOfBirth(validParameters.DateOfBirthFrom, validParameters.DateOfBirthTo);
            record.Balance = randomGenerator.Next(validParameters.BalanceMinValue);
            record.Experience = Convert.ToInt16(randomGenerator.Next(validParameters.ExperienceMinValue, validParameters.ExperienceMaxValue));
            record.EnglishLevel = GenerateEnglishLevel(validParameters.EnglishLevels);

            return record;
        }

        private string GenerateName(int min, int max)
        {
            var sb = new StringBuilder();
            var random = new Random();
            int countSymbols = this.randomGenerator.Next(min, max);

            for (int i = 0; i < countSymbols - 1; i++)
            {
                int position = random.Next(alphabet.Length - 1);
                sb.Append(i == 0 ? char.ToUpper(alphabet[position], CultureInfo.InvariantCulture) : alphabet[position]);
            }

            return sb.ToString();
        }

        private DateTime GenerateDateOfBirth(DateTime from, DateTime to)
        {
            return from.AddDays(randomGenerator.Next((to - from).Days));
        }

        private char GenerateEnglishLevel(string levels)
        {
            return levels[this.randomGenerator.Next(0, levels.Length -1)];
        }
    }
}
