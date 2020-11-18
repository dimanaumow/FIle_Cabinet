using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Date of birth validator.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">The start date.</param>
        /// <param name="to">The end date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            if (to <= from)
            {
                throw new ArgumentException($"{nameof(from)} must be early than {nameof(to)}");
            }

            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="parameters">The Record data.</param>
        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (parameters.DateOfBirth < this.from || parameters.DateOfBirth > this.to)
            {
                throw new ArgumentException($"{nameof(parameters.DateOfBirth)} is incorrect.");
            }
        }
    }
}
