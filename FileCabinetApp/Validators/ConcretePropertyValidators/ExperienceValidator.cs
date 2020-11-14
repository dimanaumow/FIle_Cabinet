using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ExperienceValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        public ExperienceValidator(int min, int max)
        {
            if (max <= min)
            {
                throw new ArgumentException($"{nameof(min)} must be less than {nameof(max)}");
            }

            this.max = max;
            this.min = min;
        }

        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (parameters.experience < this.min || parameters.experience > this.max)
            {
                throw new ArgumentException($"{nameof(parameters.experience)} must be in range from {nameof(this.min)} to {nameof(this.max)}.");
            }
        }
    }
}
