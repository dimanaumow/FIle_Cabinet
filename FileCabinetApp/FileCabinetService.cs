using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper()))
            {
                this.firstNameDictionary[record.FirstName.ToUpper()].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToUpper(), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper()))
            {
                this.lastNameDictionary[record.LastName.ToUpper()].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToUpper(), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }

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

            this.list[id - 1] = record;

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper()))
            {
                this.firstNameDictionary[record.FirstName.ToUpper()].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToUpper(), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper()))
            {
                this.lastNameDictionary[record.LastName.ToUpper()].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToUpper(), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpper()))
            {
                return this.firstNameDictionary[firstName.ToUpper()].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpper()))
            {
                return this.lastNameDictionary[lastName.ToUpper()].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            int month = int.Parse(dateOfBirth.Substring(0, 2));
            int day = int.Parse(dateOfBirth.Substring(3, 2));
            int year = int.Parse(dateOfBirth.Substring(6, 4));

            var key = new DateTime(year, month, day);

            if (this.dateOfBirthDictionary.ContainsKey(key))
            {
                return this.dateOfBirthDictionary[key].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
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
