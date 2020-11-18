using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The experience validator.
    /// </summary>
    public class ExperienceValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperienceValidator"/> class.
        /// </summary>
        /// <param name="min">The min experience.</param>
        /// <param name="max">The max experience.</param>
        public ExperienceValidator(int min, int max)
        {
            if (max <= min)
            {
                throw new ArgumentException($"{nameof(min)} must be less than {nameof(max)}");
            }

            this.max = max;
            this.min = min;
        }

        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="parameters">The record data.</param>
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
