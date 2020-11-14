using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        public IRecordValidator Create()
            => new CompositeValidator(this.validators);

        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        public ValidatorBuilder ValidateExperience(short min, short max)
        {
            this.validators.Add(new ExperienceValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateBalance(decimal min)
        {
            this.validators.Add(new BalanceValidator(min));
            return this;
        }

        public ValidatorBuilder ValidateEnglishLevel()
        {
            this.validators.Add(new EnglishLevelValidator());
            return this;
        }
    }
}
