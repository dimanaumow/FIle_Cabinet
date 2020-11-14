using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            if (maxLength <= minLength)
            {
                throw new ArgumentException($"{nameof(minLength)} must be less than {nameof(maxLength)}");
            }

            this.maxLength = maxLength;
            this.minLength = minLength;
        }

        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(parameters.firstName))
            {
                if (parameters.firstName is null)
                {
                    throw new ArgumentNullException($"{nameof(parameters.firstName)} cannot be null.");
                }

                if (parameters.firstName.Length < this.minLength || parameters.firstName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(parameters.firstName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parameters.firstName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
