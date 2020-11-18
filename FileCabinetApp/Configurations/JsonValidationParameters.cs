using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Configurations
{
    /// <summary>
    /// Incapsulate validation parameters.
    /// </summary>
    public class JsonValidationParameters
    {
        /// <summary>
        /// Gets or sets max length firstName.
        /// </summary>
        /// <value>The max length firstName.</value>
        public int FirstNameMaxLenght { get; set; }

        /// <summary>
        /// Gets or sets min length firstName.
        /// </summary>
        /// <value>The min length lastName.</value>
        public int FirstNameMinLength { get; set; }

        /// <summary>
        /// Gets or sets max length lastName.
        /// </summary>
        /// <value>The max length lastName.</value>
        public int LastNameMaxLength { get; set; }

        /// <summary>
        /// Gets or sets min length lastName.
        /// </summary>
        /// <value>The min length lastName.</value>
        public int LastNameMinLength { get; set; }

        /// <summary>
        /// Gets or sets start date of birth.
        /// </summary>
        /// <value>The start date of birth.</value>
        public DateTime DateOfBirthFrom { get; set; }

        /// <summary>
        /// Gets or sets end date of birth.
        /// </summary>
        /// <value>The end date of birth.</value>
        public DateTime DateOfBirthTo { get; set; }

        /// <summary>
        /// Gets or sets max experience.
        /// </summary>
        /// <value>The max experience.</value>
        public short ExperienceMaxValue { get; set; }

        /// <summary>
        /// Gets or sets min experience.
        /// </summary>
        /// <value>The min experience.</value>
        public short ExperienceMinValue { get; set; }

        /// <summary>
        /// Gets or sets min balance.
        /// </summary>
        /// <value>The min balance.</value>
        public int BalanceMinValue { get; set; }

        /// <summary>
        /// Gets or sets levels.
        /// </summary>
        /// <value>The levels.</value>
        public string EnglishLevels { get; set; }
    }
}
