using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Implements IFileCabinetRecord interface.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        public FileCabinetMemoryService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">Validator rule.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
            : this()
        {
            if (validator is null)
            {
                throw new ArgumentException($"{nameof(validator)} cannot be null.");
            }

            this.validator = validator;
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="parameters">Parameters of new record.</param>
        /// <returns>Id of record.</returns>
        public int CreateRecord(RecordData parameters)
        {
            this.validator.ValidatePararmeters(parameters);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = parameters.firstName,
                LastName = parameters.lastName,
                DateOfBirth = parameters.dateOfBirth,
                Experience = parameters.experience,
                Balance = parameters.balance,
                EnglishLevel = parameters.englishLevel,
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
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="parameters">Parameters of record.</param>
        public void EditRecord(int id, RecordData parameters)
        {
            if (id > this.list.Count)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            this.validator.ValidatePararmeters(parameters);

            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = parameters.firstName,
                LastName = parameters.lastName,
                DateOfBirth = parameters.dateOfBirth,
                Experience = parameters.experience,
                Balance = parameters.balance,
                EnglishLevel = parameters.englishLevel,
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
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The array of finded records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpper()))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName.ToUpper()]);
            }
            else
            {
                return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The array of finded records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpper()))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName.ToUpper()]);
            }
            else
            {
                return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The array of finded records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            int month = int.Parse(dateOfBirth.Substring(0, 2));
            int day = int.Parse(dateOfBirth.Substring(3, 2));
            int year = int.Parse(dateOfBirth.Substring(6, 4));

            var key = new DateTime(year, month, day);

            if (this.dateOfBirthDictionary.ContainsKey(key))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[key]);
            }
            else
            {
                return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            }
        }

        public bool Remove(int id)
        {
            if (id > this.list.Count)
            {
                return false;
            }

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName.ToUpper()].Remove(record);
                    this.lastNameDictionary[record.LastName.ToUpper()].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <returns>The count of records.</returns>
        public (int real, int removed) GetStat()
        {
            return (this.list.Count, 0);
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException($"{nameof(snapshot)} cannot be null.");
            }

            int count = 0;
            foreach (var record in snapshot.Records)
            {
                try
                {
                    int id = record.Id;
                    if (id <= 0)
                    {
                        throw new ArgumentOutOfRangeException($"{nameof(id)} must be positive.");
                    }

                    if (id <= this.list.Count)
                    {
                        var data = new RecordData();
                        data.firstName = record.FirstName;
                        data.lastName = record.LastName;
                        data.dateOfBirth = record.DateOfBirth;
                        data.balance = record.Balance;
                        data.experience = record.Experience;
                        data.englishLevel = record.EnglishLevel;
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
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

                        count++;
                    }
                }
                catch (IndexOutOfRangeException indexOutOfRangeException)
                {
                    Console.WriteLine($"Import record with id {record.Id} failed: {indexOutOfRangeException.Message}");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Import record with id {record.Id} failed: {exception.Message}");
                }
            }

            return count;
        }

        public void Purge()
        {
        }
    }
}
