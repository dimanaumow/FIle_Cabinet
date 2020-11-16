using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        public const string FirstName = "firstName";
        public const string LastName = "lastName";
        public const string DateOfBirth = "dateOfBirth";
        public const string Expirience = "expirience";
        public const string Balance = "balance";
        public const string EnglishLevel = "englishLevel";

        public const int LengtOfString = 120;
        public const int RecordSize = 518;

        private const short isActiveRecord = 0;
        private const short isRemovedRecord = 1;

        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;
        private readonly IRecordValidator validator;

        private Dictionary<int, long> activeRecocrds;
        private Dictionary<int, long> removedRecords;

        private bool disposed;

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

        public IEnumerable<FileCabinetRecord> FindBy(string propertyName, string value)
        {
            if (string.Equals(propertyName, FirstName, StringComparison.OrdinalIgnoreCase))
            {
                var records = this.GetRecordsCollection();

                foreach (var record in records)
                {
                    if (string.Equals(record.FirstName, value, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return record;
                    }
                }
            }
            else if (string.Equals(propertyName, LastName, StringComparison.OrdinalIgnoreCase))
            {
                var records = this.GetRecordsCollection();

                foreach (var record in records)
                {
                    if (string.Equals(record.LastName, value, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return record;
                    }
                }
            }
            else if (string.Equals(propertyName, DateOfBirth, StringComparison.OrdinalIgnoreCase))
            {
                var records = this.GetRecordsCollection();

                int month = int.Parse(value.Substring(0, 2));
                int day = int.Parse(value.Substring(3, 2));
                int year = int.Parse(value.Substring(6, 4));

                var key = new DateTime(year, month, day);

                foreach (var record in records)
                {
                    if (record.DateOfBirth == key)
                    {
                        yield return record;
                    }
                }
            }
            else if (string.Equals(propertyName, Expirience, StringComparison.OrdinalIgnoreCase))
            {
                short exp = short.Parse(value);

                foreach (var record in this.GetRecords())
                {
                    if (record.Experience == exp)
                    {
                        yield return record;
                    }
                }
            }
            else if (string.Equals(propertyName, Balance, StringComparison.OrdinalIgnoreCase))
            {
                decimal bal = decimal.Parse(value);

                foreach (var record in this.GetRecords())
                {
                    if (record.Balance == bal)
                    {
                        yield return record;
                    }
                }
            }
            else if (string.Equals(propertyName, EnglishLevel, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var record in this.GetRecords())
                {
                    if (record.EnglishLevel == value[0])
                    {
                        yield return record;
                    }
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (string.Equals(record.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (string.Equals(record.LastName, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var records = this.GetRecordsCollection();

            int month = int.Parse(dateOfBirth.Substring(0, 2));
            int day = int.Parse(dateOfBirth.Substring(3, 2));
            int year = int.Parse(dateOfBirth.Substring(6, 4));

            var key = new DateTime(year, month, day);

            foreach (var record in records)
            {
                if (record.DateOfBirth == key)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience)
        {
            short exp = short.Parse(expirience);

            foreach (var record in this.GetRecords())
            {
                if (record.Experience == exp)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByBalance(string balance)
        {
            decimal bal = decimal.Parse(balance);

            foreach (var record in this.GetRecords())
            {
                if (record.Balance == bal)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel)
        {
            foreach (var record in this.GetRecords())
            {
                if (record.EnglishLevel == englishLevel[0])
                {
                    yield return record;
                }
            }
        }

        public bool Remove(int id)
        {
            if (!this.activeRecocrds.ContainsKey(id))
            {
                return false;
            }

            long position = this.activeRecocrds[id];
            this.binWriter.BaseStream.Position = position;
            this.binWriter.Write(isRemovedRecord);

            this.activeRecocrds.Remove(id);
            this.removedRecords.Add(id, position);
            return true;
        }

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
                    balance = recod.Balance,
                    dateOfBirth = recod.DateOfBirth,
                    firstName = recod.FirstName,
                    lastName = recod.LastName,
                    experience = recod.Experience,
                    englishLevel = recod.EnglishLevel,
                };

                this.WriteRecordToBinaryFile(currentPosition, data, id);
                currentPosition += RecordSize;
            }

            this.fileStream.SetLength(currentPosition);
            this.removedRecords.Clear();
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var record in this.GetRecordsCollection())
            {
                yield return record;
            }
        }

        public (int active, int removed) GetStat()
            => (this.activeRecocrds.Count, this.removedRecords.Count);

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
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
                    var data = new RecordData();
                    data.firstName = record.FirstName;
                    data.lastName = record.LastName;
                    data.dateOfBirth = record.DateOfBirth;
                    data.balance = record.Balance;
                    data.experience = record.Experience;
                    data.englishLevel = record.EnglishLevel;

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
                catch (Exception exception)
                {
                    Console.WriteLine($"Import record with id {record.Id} failed: {exception.Message}");
                }
            }

            return count;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
            this.binWriter.Write(isActiveRecord);
            this.binWriter.Write(id);
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.firstName, new string(' ', LengtOfString - parameters.firstName.Length)).ToCharArray()));
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.lastName, new string(' ', LengtOfString - parameters.lastName.Length)).ToCharArray()));
            this.binWriter.Write(parameters.dateOfBirth.Month);
            this.binWriter.Write(parameters.dateOfBirth.Day);
            this.binWriter.Write(parameters.dateOfBirth.Year);
            this.binWriter.Write(parameters.experience);
            this.binWriter.Write(parameters.balance);
            this.binWriter.Write(Encoding.Unicode.GetBytes(parameters.englishLevel.ToString(CultureInfo.InvariantCulture)));
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
