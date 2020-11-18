using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Build Validator.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Create validator.
        /// </summary>
        /// <returns>The created validator.</returns>
        public IRecordValidator Create()
            => new CompositeValidator(this.validators);

        /// <summary>
        /// Return the firstName validator.
        /// </summary>
        /// <param name="min">The min firstName length.</param>
        /// <param name="max">The max firstName length.</param>
        /// <returns>The firstName validator.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Return the lastName validator.
        /// </summary>
        /// <param name="min">The min lastName length.</param>
        /// <param name="max">The max lastName length.</param>
        /// <returns>The lastName validator.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Return the dateOfBirth validator.
        /// </summary>
        /// <param name="from">The start date.</param>
        /// <param name="to">The end date.</param>
        /// <returns>The date of birth validator.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Return experience validator.
        /// </summary>
        /// <param name="min">The min experience.</param>
        /// <param name="max">The max experience.</param>
        /// <returns>The experience validator.</returns>
        public ValidatorBuilder ValidateExperience(short min, short max)
        {
            this.validators.Add(new ExperienceValidator(min, max));
            return this;
        }

        /// <summary>
        /// Return balance vaildator.
        /// </summary>
        /// <param name="min">The min balance.</param>
        /// <returns>The balance validator.</returns>
        public ValidatorBuilder ValidateBalance(decimal min)
        {
            this.validators.Add(new BalanceValidator(min));
            return this;
        }

        /// <summary>
        /// Return english level validator.
        /// </summary>
        /// <param name="levels">The levels.</param>
        /// <returns>The english level validator.</returns>
        public ValidatorBuilder ValidateEnglishLevel(string levels)
        {
            this.validators.Add(new EnglishLevelValidator(levels));
            return this;
        }
    }
}
