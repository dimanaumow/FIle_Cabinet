using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            if (to <= from)
            {
                throw new ArgumentException($"{nameof(from)} must be early than {nameof(to)}");
            }

            this.from = from;
            this.to = to;
        }

        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (parameters.dateOfBirth < this.from || parameters.dateOfBirth > this.to)
            {
                throw new ArgumentException($"{nameof(parameters.dateOfBirth)} is incorrect.");
            }
        }
    }
}
