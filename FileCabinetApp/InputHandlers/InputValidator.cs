using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.InputHandlers
{
    public static class InputValidator
    {
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

        public static Func<string, Tuple<bool, string, string>> stringConvrter = input =>
        {
            return new Tuple<bool, string, string>(true, input, input);
        };

        public static Func<string, Tuple<bool, string, DateTime>> dateConvrter = input =>
        {
            DateTime date;
            bool isValid = DateTime.TryParse(input, new CultureInfo("en-US"), DateTimeStyles.None, out date);

            return new Tuple<bool, string, DateTime>(isValid, input, date);
        };

        public static Func<string, Tuple<bool, string, short>> experienceConverter = input =>
        {
            short experience;
            bool isValid = short.TryParse(input, out experience);

            return new Tuple<bool, string, short>(isValid, input, experience);
        };

        public static Func<string, Tuple<bool, string, decimal>> balanceConverter = input =>
        {
            decimal balance;
            bool isValid = decimal.TryParse(input, out balance);

            return new Tuple<bool, string, decimal>(isValid, input, balance);
        };

        public static Func<string, Tuple<bool, string, char>> englishLevelConverter = input =>
        {
            char englishLevel;
            bool isValid = char.TryParse(input, out englishLevel);

            return new Tuple<bool, string, char>(isValid, input, englishLevel);
        };

        public static Func<string, Tuple<bool, string>> firstNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        public static Func<string, Tuple<bool, string>> lastNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        public static Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = date =>
        {
            bool isValid = !(date < new DateTime(1950, 1, 1) || date > DateTime.Now);
            return new Tuple<bool, string>(isValid, date.ToString());
        };

        public static Func<short, Tuple<bool, string>> experienceValidator = input =>
        {
            bool isValid = input >= 0;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        public static Func<decimal, Tuple<bool, string>> balanceValidator = input =>
        {
            bool isValid = input >= 0;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        public static Func<char, Tuple<bool, string>> englishLevelValidator = input =>
        {
            bool isValid = input == 'a' || input == 'c' || input == 'b';
            return new Tuple<bool, string>(isValid, input.ToString());
        };
    }
}
