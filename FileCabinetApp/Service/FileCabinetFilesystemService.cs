using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// FileSystem service.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private const int LengtOfString = 120;
        private const int RecordSize = 518;

        private const short IsActiveRecord = 0;
        private const short IsRemovedRecord = 1;

        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;
        private readonly IRecordValidator validator;

        private Dictionary<int, long> activeRecocrds;
        private Dictionary<int, long> removedRecords;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="fileStream">The filestream</param>
        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            if (validator is null)
            {
                throw new ArgumentNullException($"{nameof(validator)} cannot be null.");
            }

            if (fileStream is null)
            {
                throw new ArgumentNullException($"{nameof(fileStream)} cannot be null.");
            }

            this.validator = validator;

            this.fileStream = fileStream;
            this.binReader = new BinaryReader(fileStream);
            this.binWriter = new BinaryWriter(fileStream);

            this.activeRecocrds = new Dictionary<int, long>();
            this.removedRecords = new Dictionary<int, long>();

            this.disposed = true;
        }

        /// <summary>
        /// Create record.
        /// </summary>
        /// <param name="parameters">Record data.</param>
        /// <returns>Created record.</returns>
        public int CreateRecord(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentException($"{nameof(parameters)} cannot be null.");
            }

            this.validator.ValidatePararmeters(parameters);
            int? id = this.GetCurrentId();

            if (id is null)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} bigger than int.MaxValue.");
            }

            return this.CreateRecordWithId(parameters, id.Value);
        }

        /// <summary>
        /// Edit record with given id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="parameters">Record data.</param>
        public void EditRecord(int id, RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            this.validator.ValidatePararmeters(parameters);

            if (!this.activeRecocrds.ContainsKey(id))
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            this.WriteRecordToBinaryFile(this.activeRecocrds[id], parameters, id);
        }

        /// <summary>
        /// Find by firstName.
        /// </summary>
        /// <param name="firstName">FirstName.</param>
        /// <returns>The records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} cannot be null.");
            }

            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (string.Equals(record.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find by lastName.
        /// </summary>
        /// <param name="lastName">LastName.</param>
        /// <returns>The record sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} cannot be null.");
            }

            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (string.Equals(record.LastName, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find by date.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>The record sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (dateOfBirth is null)
            {
                throw new ArgumentNullException($"{nameof(dateOfBirth)} cannot be null.");
            }

            var records = this.GetRecordsCollection();

            int month = int.Parse(dateOfBirth.Substring(0, 2), CultureInfo.InvariantCulture);
            int day = int.Parse(dateOfBirth.Substring(3, 2), CultureInfo.InvariantCulture);
            int year = int.Parse(dateOfBirth.Substring(6, 4), CultureInfo.InvariantCulture);

            var key = new DateTime(year, month, day);

            foreach (var record in records)
            {
                if (record.DateOfBirth == key)
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find by experience.
        /// </summary>
        /// <param name="expirience">Experience.</param>
        /// <returns>The record sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience)
        {
            if (expirience is null)
            {
                throw new ArgumentNullException($"{nameof(expirience)} cannot be null.");
            }

            short exp = short.Parse(expirience, CultureInfo.InvariantCulture);

            foreach (var record in this.GetRecords())
            {
                if (record.Experience == exp)
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find by balance.
        /// </summary>
        /// <param name="balance">Balance.</param>
        /// <returns>The record sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByBalance(string balance)
        {
            if (balance is null)
            {
                throw new ArgumentNullException($"{nameof(balance)} cannot be null.");
            }

            decimal bal = decimal.Parse(balance, CultureInfo.InvariantCulture);

            foreach (var record in this.GetRecords())
            {
                if (record.Balance == bal)
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find by level.
        /// </summary>
        /// <param name="englishLevel">English level.</param>
        /// <returns>The recordsequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel)
        {
            if (englishLevel is null)
            {
                throw new ArgumentNullException($"{nameof(englishLevel)} cannot be null.");
            }

            foreach (var record in this.GetRecords())
            {
                if (record.EnglishLevel == englishLevel[0])
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Remove record by given id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Is remove.</returns>
        public bool Remove(int id)
        {
            if (!this.activeRecocrds.ContainsKey(id))
            {
                return false;
            }

            long position = this.activeRecocrds[id];
            this.binWriter.BaseStream.Position = position;
            this.binWriter.Write(IsRemovedRecord);

            this.activeRecocrds.Remove(id);
            this.removedRecords.Add(id, position);
            return true;
        }

        /// <summary>
        /// Remove deleted record from file.
        /// </summary>
        public void Purge()
        {
            long currentPosition = 0;
            var activePositions = this.activeRecocrds.Values.ToArray();
            Array.Sort(activePositions);

            foreach (var activePosition in activePositions)
            {
                var recod = this.ReadRecordOutBinaryFile(activePosition);

                int id = recod.Id;
                var data = new RecordData()
                {
                    Balance = recod.Balance,
                    DateOfBirth = recod.DateOfBirth,
                    FirstName = recod.FirstName,
                    LastName = recod.LastName,
                    Experience = recod.Experience,
                    EnglishLevel = recod.EnglishLevel,
                };

                this.WriteRecordToBinaryFile(currentPosition, data, id);
                currentPosition += RecordSize;
            }

            this.fileStream.SetLength(currentPosition);
            this.removedRecords.Clear();
        }

        /// <summary>
        /// Get all record.
        /// </summary>
        /// <returns>All records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var record in this.GetRecordsCollection())
            {
                yield return record;
            }
        }

        /// <summary>
        /// Get count active and removes record.
        /// </summary>
        /// <returns>count active and removes record.</returns>
        public (int active, int removed) GetStat()
            => (this.activeRecocrds.Count, this.removedRecords.Count);

        /// <summary>
        /// Make snapshot records.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
        }

        /// <summary>
        /// Add records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <returns>Count of restored records.</returns>
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
                    var data = new RecordData();
                    data.FirstName = record.FirstName;
                    data.LastName = record.LastName;
                    data.DateOfBirth = record.DateOfBirth;
                    data.Balance = record.Balance;
                    data.Experience = record.Experience;
                    data.EnglishLevel = record.EnglishLevel;

                    if (this.GetRecordsCollection().Any(item => item.Id == record.Id))
                    {
                        this.EditRecord(record.Id, data);
                        count++;
                    }
                    else
                    {
                        this.CreateRecordWithId(data, record.Id);
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
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.binReader.Close();
                this.binWriter.Close();
                this.fileStream.Close();
            }

            this.disposed = true;
        }

        private int CreateRecordWithId(RecordData parameters, int id)
        {
            this.validator.ValidatePararmeters(parameters);

            if (this.activeRecocrds.ContainsKey(id))
            {
                throw new ArgumentException($"#{nameof(id)} is not unique.");
            }

            long position = this.removedRecords.ContainsKey(id) ? this.removedRecords[id] : this.fileStream.Length;
            this.activeRecocrds.Add(id, position);
            this.removedRecords.Remove(id);
            this.WriteRecordToBinaryFile(position, parameters, id);

            return id;
        }

        private void WriteRecordToBinaryFile(long position, RecordData parameters, int id)
        {
            this.binWriter.Seek((int)position, SeekOrigin.Begin);
            this.binWriter.Write(IsActiveRecord);
            this.binWriter.Write(id);
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.FirstName, new string(' ', LengtOfString - parameters.FirstName.Length)).ToCharArray()));
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.LastName, new string(' ', LengtOfString - parameters.LastName.Length)).ToCharArray()));
            this.binWriter.Write(parameters.DateOfBirth.Month);
            this.binWriter.Write(parameters.DateOfBirth.Day);
            this.binWriter.Write(parameters.DateOfBirth.Year);
            this.binWriter.Write(parameters.Experience);
            this.binWriter.Write(parameters.Balance);
            this.binWriter.Write(Encoding.Unicode.GetBytes(parameters.EnglishLevel.ToString(CultureInfo.InvariantCulture)));
        }

        private FileCabinetRecord ReadRecordOutBinaryFile(long position)
        {
            this.binReader.BaseStream.Position = position;
            short readKey = this.binReader.ReadInt16();

            var record = new FileCabinetRecord()
            {
                Id = this.binReader.ReadInt32(),
                FirstName = Encoding.Unicode.GetString(this.binReader.ReadBytes(LengtOfString * 2)).Trim(),
                LastName = Encoding.Unicode.GetString(this.binReader.ReadBytes(LengtOfString * 2)).Trim(),
                DateOfBirth = DateTime.Parse($"{this.binReader.ReadInt32()}/{this.binReader.ReadInt32()}/{this.binReader.ReadInt32()}", CultureInfo.InvariantCulture),
                Experience = this.binReader.ReadInt16(),
                Balance = this.binReader.ReadDecimal(),
                EnglishLevel = Encoding.Unicode.GetString(this.binReader.ReadBytes(sizeof(char))).First(),
            };

            return record;
        }

        private List<FileCabinetRecord> GetRecordsCollection()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            var activePositions = this.activeRecocrds.Values.ToArray();
            Array.Sort(activePositions);

            foreach (var activePosition in activePositions)
            {
                var record = this.ReadRecordOutBinaryFile(activePosition);
                records.Add(record);
            }

            return records;
        }

        private int? GetCurrentId()
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!this.activeRecocrds.ContainsKey(i))
                {
                    return i;
                }
            }

            return null;
        }
    }
}
