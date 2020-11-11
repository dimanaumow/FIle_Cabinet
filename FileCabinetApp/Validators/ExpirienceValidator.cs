using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ExpirienceValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        public ExpirienceValidator(int min, int max)
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

            if (parameters.expirience < this.min || parameters.expirience > this.max)
            {
                throw new ArgumentException($"{nameof(parameters.expirience)} must be in range from {nameof(min)} to {nameof(max)}.");
            }
        }
    }
}
