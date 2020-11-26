using System;
using System.Globalization;

#pragma warning disable
namespace FileCabinetApp.InputHandlers
{
    /// <summary>
    /// Input handlers.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Read input data.
        /// </summary>
        /// <typeparam name="T">Data.</typeparam>
        /// <param name="converter">Data converter delegate.</param>
        /// <param name="validator">Data validator delegate.</param>
        /// <returns>Validated data.</returns>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        #region Converters

        /// <summary>
        /// String converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, string>> stringConvrter = input =>
        {
            return new Tuple<bool, string, string>(true, "Succes", input);
        };

        /// <summary>
        /// DateOfBirth converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, DateTime>> dateConvrter = input =>
        {
            DateTime date;
            bool isValid = DateTime.TryParseExact(input, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out date);

            return new Tuple<bool, string, DateTime>(isValid, "Incorrect date format.", date);
        };

        /// <summary>
        /// Expirience converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, short>> experienceConverter = input =>
        {
            short experience;
            bool isValid = short.TryParse(input, out experience);

            return new Tuple<bool, string, short>(isValid, input, experience);
        };

        /// <summary>
        /// Balance converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, decimal>> balanceConverter = input =>
        {
            decimal balance;
            bool isValid = decimal.TryParse(input, out balance);

            return new Tuple<bool, string, decimal>(isValid, input, balance);
        };

        /// <summary>
        /// EnglishLevel converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, char>> englishLevelConverter = input =>
        {
            char englishLevel;
            bool isValid = char.TryParse(input, out englishLevel);

            return new Tuple<bool, string, char>(isValid, input, englishLevel);
        };

        #endregion

        #region Validators
        /// <summary>
        /// FirstName validator.
        /// </summary>
        public static Func<string, Tuple<bool, string>> firstNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        /// <summary>
        /// LastName validator.
        /// </summary>
        public static Func<string, Tuple<bool, string>> lastNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        /// <summary>
        /// DateOfBirth validator.
        /// </summary>
        public static Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = date =>
        {
            bool isValid = !(date < new DateTime(1950, 1, 1) || date > DateTime.Now);
            return new Tuple<bool, string>(isValid, date.ToString());
        };

        /// <summary>
        /// Expirience validator.
        /// </summary>
        public static Func<short, Tuple<bool, string>> experienceValidator = input =>
        {
            bool isValid = input >= 0;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        /// <summary>
        /// Balance validator.
        /// </summary>
        public static Func<decimal, Tuple<bool, string>> balanceValidator = input =>
        {
            bool isValid = input >= 0;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        /// <summary>
        /// English level validator.
        /// </summary>
        public static Func<char, Tuple<bool, string>> englishLevelValidator = input =>
        {
            bool isValid = input == 'a' || input == 'c' || input == 'b';
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        #endregion
    }
}
