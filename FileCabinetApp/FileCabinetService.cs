using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Provide service for work with user's record.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Generates a unique user record.
        /// </summary>
        /// <param name="parameters">Parameters of new record.</param>
        /// <returns>Id of record.</returns>
        public int CreateRecord(RecordData parameters)
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

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = parameters.firstName,
                LastName = parameters.lastName,
                DateOfBirth = parameters.dateOfBirth,
                Expirience = parameters.expirience,
                Balance = parameters.balance,
                Nationality = parameters.nationality,
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

        /// <summary>
        /// Changes the record by given ID.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="parametrs">Parameters of record.</param>
        public void EditRecord(int id, RecordData parametrs)
        {
            if (id > this.list.Count)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            if (string.IsNullOrWhiteSpace(parametrs.firstName))
            {
                if (parametrs.firstName is null)
                {
                    throw new ArgumentNullException($"{nameof(parametrs.firstName)} cannot be null.");
                }

                if (parametrs.firstName.Length < 2 || parametrs.firstName.Length > 60)
                {
                    throw new ArgumentException($"{nameof(parametrs.firstName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parametrs.firstName)} cannot be empty or whiteSpace.");
            }

            if (string.IsNullOrWhiteSpace(parametrs.lastName))
            {
                if (parametrs.lastName is null)
                {
                    throw new ArgumentNullException($"{nameof(parametrs.lastName)} cannot be null.");
                }

                if (parametrs.lastName.Length < 2 || parametrs.lastName.Length > 60)
                {
                    throw new ArgumentException($"{nameof(parametrs.lastName.Length)} must be in range 2 to 60.");
                }

                throw new ArgumentException($"{nameof(parametrs.lastName)} cannot be empty or whiteSpace.");
            }

            if (parametrs.dateOfBirth < new DateTime(1950, 1, 1) || parametrs.dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(parametrs.dateOfBirth)} is incorrect.");
            }

            if (parametrs.expirience < 0 || parametrs.expirience > DateTime.Now.Year - parametrs.dateOfBirth.Year)
            {
                throw new ArgumentException($"{nameof(parametrs.expirience)} must be positive and less than year of life.");
            }

            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = parametrs.firstName,
                LastName = parametrs.lastName,
                DateOfBirth = parametrs.dateOfBirth,
                Expirience = parametrs.expirience,
                Balance = parametrs.balance,
                Nationality = parametrs.nationality,
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

        /// <summary>
        /// Find all records with given firstName.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The array of finded records.</returns>
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

        /// <summary>
        /// Find all records with given lastName.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The array of finded records.</returns>
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

        /// <summary>
        /// Find all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The array of finded records.</returns>
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

        /// <summary>
        /// Give all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Give the count of records.
        /// </summary>
        /// <returns>The count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
