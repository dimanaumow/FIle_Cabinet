using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Implement IRecordValidator interface.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validate record parameters using custom rules.
        /// </summary>
        /// <param name="parameters">User's data</param>
        public void ValidatePararmeters(RecordData parameters)
        {
            this.ValidateFirstName(parameters.firstName);
            this.ValidateLastName(parameters.lastName);
            this.ValidateDateOfBirth(parameters.dateOfBirth);
            this.ValidateExpirience(parameters.expirience);
            this.ValidateBalance(parameters.balance);
            this.ValidateNationality(parameters.nationality);
        }

        private void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                if (firstName is null)
                {
                    throw new ArgumentNullException($"{nameof(firstName)} cannot be null.");
                }

                if (firstName.Length < 2 || firstName.Length > 60)
                {
                    throw new ArgumentException($"{nameof(firstName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(firstName)} cannot be empty or whiteSpace.");
            }
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                if (lastName is null)
                {
                    throw new ArgumentNullException($"{nameof(lastName)} cannot be null.");
                }

                if (lastName.Length < 2 || lastName.Length > 60)
                {
                    throw new ArgumentException($"{nameof(lastName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(lastName)} cannot be empty or whiteSpace.");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is incorrect.");
            }
        }

        private void ValidateExpirience(short expirience)
        {
            if (expirience < 0)
            {
                throw new ArgumentException($"{nameof(expirience)} must be positive.");
            }
        }

        private void ValidateBalance(decimal balance)
        {
            if (balance < 0)
            {
                throw new ArgumentException($"{nameof(balance)} must be positive.");
            }
        }

        private void ValidateNationality(char nationality)
        {
            if (!char.IsLetter(nationality))
            {
                throw new ArgumentException($"{nameof(nationality)} must be letter.");
            }
        }
    }
}
