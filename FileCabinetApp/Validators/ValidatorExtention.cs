using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Configurations;

namespace FileCabinetApp.Validators
{
    public static class ValidatorExtention
    {
        public static IRecordValidator Create(this ValidatorBuilder builder, string validationRule = "default")
        {
            if (builder is null)
            {
                throw new ArgumentNullException($"{nameof(builder)} canot be null.");
            }

            var setters = new ConfigurationSetter(validationRule);
            var validateParameters = setters.GetParameters();

            return builder.
                        ValidateFirstName(validateParameters.FirstNameMinLength, validateParameters.FirstNameMaxLenght).
                        ValidateLastName(validateParameters.LastNameMinLength, validateParameters.LastNameMaxLength).
                        ValidateDateOfBirth(validateParameters.DateOfBirthFrom, validateParameters.DateOfBirthTo).
                        ValidateExperience(validateParameters.ExperienceMinValue, validateParameters.ExperienceMaxValue).
                        ValidateBalance(validateParameters.BalanceMinValue).
                        ValidateEnglishLevel(validateParameters.EnglishLevels).
                        Create();
        }
    }
}
