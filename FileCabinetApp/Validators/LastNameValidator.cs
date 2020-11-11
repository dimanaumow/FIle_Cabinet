﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        public LastNameValidator(int minLength, int maxLength)
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

            if (string.IsNullOrWhiteSpace(parameters.lastName))
            {
                if (parameters.lastName is null)
                {
                    throw new ArgumentNullException($"{nameof(parameters.lastName)} cannot be null.");
                }

                if (parameters.lastName.Length < this.minLength || parameters.lastName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(parameters.lastName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parameters.lastName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
