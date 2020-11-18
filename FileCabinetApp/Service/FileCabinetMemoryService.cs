using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Memoization;

#pragma warning disable CA1062
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
        public FileCabinetMemoryService()
        {
        }

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
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Experience = parameters.Experience,
                Balance = parameters.Balance,
                EnglishLevel = parameters.EnglishLevel,
            };

            this.list.Add(record);

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
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
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Experience = parameters.Experience,
                Balance = parameters.Balance,
                EnglishLevel = parameters.EnglishLevel,
            };

            this.list[id - 1] = record;

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
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
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} cannot be null.");
            }

            if (CashedData.FirstNameCashe.ContainsKey(firstName))
            {
                return CashedData.FirstNameCashe[firstName];
            }

            CashedData.FirstNameCashe.Add(firstName, this.FindFirstName(firstName));
            return this.FindFirstName(firstName);
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The array of finded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} cannot be null.");
            }

            if (CashedData.LastNameCashe.ContainsKey(lastName))
            {
                return CashedData.FirstNameCashe[lastName];
            }

            CashedData.LastNameCashe.Add(lastName, this.FindLastName(lastName));
            return this.FindLastName(lastName);
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The array of finded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (dateOfBirth is null)
            {
                throw new ArgumentNullException($"{nameof(dateOfBirth)} cannot be null.");
            }

            if (CashedData.DateOfBirtCashe.ContainsKey(dateOfBirth))
            {
                return CashedData.DateOfBirtCashe[dateOfBirth];
            }

            CashedData.DateOfBirtCashe.Add(dateOfBirth, this.FindDateOfBirth(dateOfBirth));
            return this.FindDateOfBirth(dateOfBirth);
        }

        /// <summary>
        /// Find by experience.
        /// </summary>
        /// <param name="expirience">Experience.</param>
        /// <returns>The sequance of record.</returns>
        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience)
        {
            if (expirience is null)
            {
                throw new ArgumentNullException($"{nameof(expirience)} cannot be null.");
            }

            if (CashedData.ExperienceCashe.ContainsKey(expirience))
            {
                return CashedData.ExperienceCashe[expirience];
            }

            CashedData.ExperienceCashe.Add(expirience, this.FindExpirience(expirience));
            return this.FindExpirience(expirience);
        }

        /// <summary>
        /// Find by balance.
        /// </summary>
        /// <param name="balance">Balance.</param>
        /// <returns>The sequance of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByBalance(string balance)
        {
            if (balance is null)
            {
                throw new ArgumentNullException($"{nameof(balance)} cannot be null");
            }

            if (CashedData.BalanceCashe.ContainsKey(balance))
            {
                return CashedData.BalanceCashe[balance];
            }

            CashedData.BalanceCashe.Add(balance, this.FindBalance(balance));
            return this.FindBalance(balance);
        }

        /// <summary>
        /// Find by english level.
        /// </summary>
        /// <param name="englishLevel">English level.</param>
        /// <returns>the sequance of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel)
        {
            if (englishLevel is null)
            {
                throw new ArgumentNullException($"{nameof(englishLevel)} cannot be null.");
            }

            if (CashedData.EnglishLevelCashe.ContainsKey(englishLevel))
            {
                return CashedData.EnglishLevelCashe[englishLevel];
            }

            CashedData.EnglishLevelCashe.Add(englishLevel, this.FindEnglishLevel(englishLevel));
            return this.FindEnglishLevel(englishLevel);
        }

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Is removed.</returns>
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
                    this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(record);
                    this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(record);
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
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var item in this.list)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Implements IFileCabinetRecord interface.
        /// </summary>
        /// <returns>The count of records.</returns>
        public (int active, int removed) GetStat()
        {
            return (this.list.Count, 0);
        }

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <summary>
        /// Remove deleted records from file.
        /// </summary>
        /// <param name="snapshot">Snapshot</param>
        /// <returns>Count restored records.</returns>
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
                        data.FirstName = record.FirstName;
                        data.LastName = record.LastName;
                        data.DateOfBirth = record.DateOfBirth;
                        data.Balance = record.Balance;
                        data.Experience = record.Experience;
                        data.EnglishLevel = record.EnglishLevel;
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.list.Add(record);

                        if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper(CultureInfo.InvariantCulture)))
                        {
                            this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
                        }
                        else
                        {
                            this.firstNameDictionary.Add(record.FirstName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
                        }

                        if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper(CultureInfo.InvariantCulture)))
                        {
                            this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
                        }
                        else
                        {
                            this.lastNameDictionary.Add(record.LastName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
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
            }

            return count;
        }

        /// <summary>
        /// Only file service.
        /// </summary>
        public void Purge()
        {
        }

        private IEnumerable<FileCabinetRecord> FindFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpper(CultureInfo.InvariantCulture)))
            {
                var collection = this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)];

                foreach (var item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                yield break;
            }
        }

        private IEnumerable<FileCabinetRecord> FindLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpper(CultureInfo.InvariantCulture)))
            {
                var collection = this.lastNameDictionary[lastName.ToUpper(CultureInfo.InvariantCulture)];

                foreach (var item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                yield break;
            }
        }

        private IEnumerable<FileCabinetRecord> FindDateOfBirth(string dateOfBirth)
        {
            int month = int.Parse(dateOfBirth.Substring(0, 2), CultureInfo.InvariantCulture);
            int day = int.Parse(dateOfBirth.Substring(3, 2), CultureInfo.InvariantCulture);
            int year = int.Parse(dateOfBirth.Substring(6, 4), CultureInfo.InvariantCulture);

            var key = new DateTime(year, month, day);

            if (this.dateOfBirthDictionary.ContainsKey(key))
            {
                var collection = this.dateOfBirthDictionary[key];

                foreach (var item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                yield break;
            }
        }

        private IEnumerable<FileCabinetRecord> FindExpirience(string expirience)
        {
            short exp = short.Parse(expirience, CultureInfo.InvariantCulture);

            foreach (var record in this.GetRecords())
            {
                if (record.Experience == exp)
                {
                    yield return record;
                }
            }
        }

        private IEnumerable<FileCabinetRecord> FindBalance(string balance)
        {
            decimal bal = decimal.Parse(balance, CultureInfo.InvariantCulture);

            foreach (var record in this.GetRecords())
            {
                if (record.Balance == bal)
                {
                    yield return record;
                }
            }
        }

        private IEnumerable<FileCabinetRecord> FindEnglishLevel(string englishLevel)
        {
            foreach (var record in this.GetRecords())
            {
                if (record.EnglishLevel == englishLevel[0])
                {
                    yield return record;
                }
            }
        }
    }
}
