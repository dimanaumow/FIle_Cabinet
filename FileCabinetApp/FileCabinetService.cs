using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short expirience, decimal balance, char nationality)
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

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is incorrect.");
            }

            if (expirience < 0 || expirience > DateTime.Now.Year - dateOfBirth.Year)
            {
                throw new ArgumentException($"{nameof(expirience)} must be positive and less than year of life.");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Expirience = expirience,
                Balance = balance,
                Nationality = nationality
            };

            this.list.Add(record);

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short expirience, decimal balance, char nationality)
        {
            if (id > this.list.Count)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

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

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is incorrect.");
            }

            if (expirience < 0 || expirience > DateTime.Now.Year - dateOfBirth.Year)
            {
                throw new ArgumentException($"{nameof(expirience)} must be positive and less than year of life.");
            }

            this.list[id - 1] = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Expirience = expirience,
                Balance = balance,
                Nationality = nationality
            };
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
