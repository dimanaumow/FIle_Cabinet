using System;
using System.Globalization;
using System.Text;
using FileCabinetApp.Configurations;
using FileCabinetApp.Service;

namespace FileCabinetApp.Generators
{
    /// <summary>
    /// Generator for FileCabinetRecord.
    /// </summary>
    public class RecordGenerator
    {
        /// <summary>
        /// Alpabet of english letters.
        /// </summary>
        public const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly Random randomGenerator;
        private readonly ConfigurationSetter setter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordGenerator"/> class.
        /// </summary>
        public RecordGenerator()
        {
            this.setter = new ConfigurationSetter("custom");
            this.randomGenerator = new Random();
        }

        /// <summary>
        /// Generate fileCabinetRecord.
        /// </summary>
        /// <param name="recordId">Record id.</param>
        /// <returns>the generated record.</returns>
        public FileCabinetRecord Generate(int recordId)
        {
            var validParameters = this.setter.GetParameters();
            var record = new FileCabinetRecord();

            record.Id = recordId;
            record.FirstName = this.GenerateName(validParameters.FirstNameMinLength, validParameters.FirstNameMaxLenght);
            record.LastName = this.GenerateName(validParameters.LastNameMinLength, validParameters.LastNameMaxLength);
            record.DateOfBirth = this.GenerateDateOfBirth(validParameters.DateOfBirthFrom, validParameters.DateOfBirthTo);
            record.Balance = this.randomGenerator.Next(validParameters.BalanceMinValue);
            record.Experience = Convert.ToInt16(this.randomGenerator.Next(validParameters.ExperienceMinValue, validParameters.ExperienceMaxValue));
            record.EnglishLevel = this.GenerateEnglishLevel(validParameters.EnglishLevels);

            return record;
        }

        private string GenerateName(int min, int max)
        {
            var sb = new StringBuilder();
            var random = new Random();
            int countSymbols = this.randomGenerator.Next(min, max);

            for (int i = 0; i < countSymbols - 1; i++)
            {
                int position = random.Next(Alphabet.Length - 1);
                sb.Append(i == 0 ? char.ToUpper(Alphabet[position], CultureInfo.InvariantCulture) : Alphabet[position]);
            }

            return sb.ToString();
        }

        private DateTime GenerateDateOfBirth(DateTime from, DateTime to)
        {
            return from.AddDays(this.randomGenerator.Next((to - from).Days));
        }

        private char GenerateEnglishLevel(string levels)
        {
            return levels[this.randomGenerator.Next(0, levels.Length - 1)];
        }
    }
}
