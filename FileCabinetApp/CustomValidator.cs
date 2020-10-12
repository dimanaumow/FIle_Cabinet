using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        public void ValidatePararmeters(RecordData parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.firstName))
            {
                if (parameters.firstName is null)
                {
                    throw new ArgumentNullException($"{nameof(parameters.firstName)} cannot be null.");
                }

                if (parameters.firstName.Length < 2 || parameters.firstName.Length > 60)
                {
                    throw new ArgumentException($"{nameof(parameters.firstName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parameters.firstName)} cannot be empty or whiteSpace.");
            }

            if (string.IsNullOrWhiteSpace(parameters.lastName))
            {
                if (parameters.lastName is null)
                {
                    throw new ArgumentNullException($"{nameof(parameters.lastName)} cannot be null.");
                }

                if (parameters.lastName.Length < 2 || parameters.lastName.Length > 60)
                {
                    throw new ArgumentException($"{nameof(parameters.lastName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parameters.lastName)} cannot be empty or whiteSpace.");
            }

            if (parameters.dateOfBirth < new DateTime(1950, 1, 1) || parameters.dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(parameters.dateOfBirth)} is incorrect.");
            }

            if (parameters.expirience < 0 || parameters.expirience > DateTime.Now.Year - parameters.dateOfBirth.Year)
            {
                throw new ArgumentException($"{nameof(parameters.expirience)} must be positive and less than year of life.");
            }

            if (parameters.balance < 0)
            {
                throw new ArgumentException($"{nameof(parameters.balance)} must be positive.");
            }
        }
    }
}
