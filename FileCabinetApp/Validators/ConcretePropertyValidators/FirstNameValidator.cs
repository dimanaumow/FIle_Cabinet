using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The firstName validator.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">The min length name.</param>
        /// <param name="maxLength">The max length name.</param>
        public FirstNameValidator(int minLength, int maxLength)
        {
            if (maxLength <= minLength)
            {
                throw new ArgumentException($"{nameof(minLength)} must be less than {nameof(maxLength)}");
            }

            this.maxLength = maxLength;
            this.minLength = minLength;
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

            if (string.IsNullOrWhiteSpace(parameters.FirstName))
            {
                if (parameters.FirstName is null)
                {
                    throw new ArgumentNullException($"{nameof(parameters.FirstName)} cannot be null.");
                }

                if (parameters.FirstName.Length < this.minLength || parameters.FirstName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(parameters.FirstName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parameters.FirstName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
