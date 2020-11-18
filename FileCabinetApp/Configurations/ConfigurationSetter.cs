using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Configurations
{
    /// <summary>
    /// Maintains setting current configurations for FileCabinetRecord options.
    /// </summary>
    public class ConfigurationSetter
    {
        private const string SettersPath = @"d:\AutocodeEPAM\FileCabinet\validation-rules.json";
        private readonly IConfiguration configuration;
        private readonly string validationRule;
        private readonly JsonValidationParameters validationParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSetter"/> class.
        /// </summary>
        /// <param name="validationRule">Is validation rule.</param>
        public ConfigurationSetter(string validationRule)
        {
            if (validationRule is null)
            {
                throw new ArgumentNullException($"{nameof(validationRule)} cannot be null.");
            }

            this.validationRule = validationRule;
            this.configuration = new ConfigurationBuilder().AddJsonFile(SettersPath).Build();
            this.validationParameters = new JsonValidationParameters();
        }

        /// <summary>
        /// Get configuration parameters.
        /// </summary>
        /// <returns>Json validation parameters.</returns>
        public JsonValidationParameters GetParameters()
        {
            this.SetParameters();
            return this.validationParameters;
        }

        private void SetParameters()
        {
            var ruleCection = this.configuration.GetSection(this.validationRule);

            this.validationParameters.FirstNameMaxLenght = ruleCection.GetSection("firstName").GetValue<int>("max");
            this.validationParameters.FirstNameMinLength = ruleCection.GetSection("firstName").GetValue<int>("min");

            this.validationParameters.LastNameMaxLength = ruleCection.GetSection("lastName").GetValue<int>("max");
            this.validationParameters.LastNameMinLength = ruleCection.GetSection("lastName").GetValue<int>("min");

            this.validationParameters.DateOfBirthFrom = ruleCection.GetSection("dateOfBirth").GetValue<DateTime>("from");
            this.validationParameters.DateOfBirthTo = ruleCection.GetSection("dateOfBirth").GetValue<DateTime>("to");

            this.validationParameters.ExperienceMaxValue = ruleCection.GetSection("experience").GetValue<short>("max");
            this.validationParameters.ExperienceMinValue = ruleCection.GetSection("experience").GetValue<short>("min");

            this.validationParameters.BalanceMinValue = ruleCection.GetSection("balance").GetValue<int>("min");

            this.validationParameters.EnglishLevels = ruleCection.GetSection("englishLevel").GetValue<string>("levels");
        }
    }
}
