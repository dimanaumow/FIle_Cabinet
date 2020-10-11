using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Provide service for work with user's record.
    /// </summary>
    public abstract class FileCabinetService
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
            this.ValidateParameters(parameters);

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
        public void EditRecord(int id, RecordData parameters)
        {
            if (id > this.list.Count)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            this.ValidateParameters(parameters);

            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = parameters.firstName,
                LastName = parameters.lastName,
                DateOfBirth = parameters.dateOfBirth,
                Expirience = parameters.expirience,
                Balance = parameters.balance,
                Nationality = parameters.nationality,
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

        /// <summary>
        /// Validate record parameters.
        /// </summary>
        /// <param name="parameters">Parameters of record.</param>
        protected abstract void ValidateParameters(RecordData parameters);
    }
}
