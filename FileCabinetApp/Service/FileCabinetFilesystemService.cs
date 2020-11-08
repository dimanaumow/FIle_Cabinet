using FileCabinetApp.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        public const int LengtOfString = 120;
        public const int RecordSize = 518;
        private const short isRealRecord = 0;
        private const short isRemovedRecord = 1;

        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;
        private readonly IRecordValidator validator;
        private int position;
        private bool disposed;
        private int id;

        private List<int> realIdRecord;
        private List<int> removeIdRecords;

        public FileCabinetFilesystemService(FileStream fileStream)
            : this(new DefaultValidator(), fileStream)
        {
        }

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
            this.disposed = true;
            this.position = 0;
            this.id = 1;
            this.realIdRecord = new List<int>();
            this.removeIdRecords = new List<int>();
        }

        public int CreateRecord(RecordData parameters)
        {
            this.validator.ValidatePararmeters(parameters);
            this.WriteRecordToBinaryFile(this.position, parameters, this.id);
            this.position += RecordSize;
            this.realIdRecord.Add(this.id);
            return this.id++;
        }

        public void EditRecord(int id, RecordData parameters)
        {
            if (!this.realIdRecord.Contains(id))
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            int position = (id - 1) * RecordSize;
            this.WriteRecordToBinaryFile(position, parameters, id);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = GetRecordsCollection();
            var result = new List<FileCabinetRecord>();

            foreach (var record in records)
            {
                if (string.Equals(record.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = GetRecordsCollection();
            var result = new List<FileCabinetRecord>();

            foreach (var record in records)
            {
                if (string.Equals(record.LastName, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var records = GetRecordsCollection();
            var result = new List<FileCabinetRecord>();

            int month = int.Parse(dateOfBirth.Substring(0, 2));
            int day = int.Parse(dateOfBirth.Substring(3, 2));
            int year = int.Parse(dateOfBirth.Substring(6, 4));

            var key = new DateTime(year, month, day);

            foreach (var record in records)
            {
                if (record.DateOfBirth == key)
                {
                    result.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public bool Remove(int id)
        {
            if (!this.realIdRecord.Contains(id))
            {
                return false;
            }

            this.binReader.BaseStream.Position = id * RecordSize;
            this.binWriter.Write(isRemovedRecord);
            this.realIdRecord.Add(id);
            this.realIdRecord.Remove(id);
            return true;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
            =>
            new ReadOnlyCollection<FileCabinetRecord>(this.GetRecordsCollection());

        public (int real, int removed) GetStat()
            => (this.realIdRecord.Count, this.removeIdRecords.Count);

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            throw new NotImplementedException();
        }

        private void WriteRecordToBinaryFile(int position, RecordData parameters, int id)
        {
            this.binWriter.Seek(position, SeekOrigin.Begin);
            this.binWriter.Write(isRealRecord);
            this.binWriter.Write(id);
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.firstName, new string(' ', LengtOfString - parameters.firstName.Length)).ToCharArray()));
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.lastName, new string(' ', LengtOfString - parameters.lastName.Length)).ToCharArray()));
            this.binWriter.Write(parameters.dateOfBirth.Month);
            this.binWriter.Write(parameters.dateOfBirth.Day);
            this.binWriter.Write(parameters.dateOfBirth.Year);
            this.binWriter.Write(parameters.expirience);
            this.binWriter.Write(parameters.balance);
            this.binWriter.Write(Encoding.Unicode.GetBytes(parameters.nationality.ToString(CultureInfo.InvariantCulture)));
        }

        private FileCabinetRecord ReadRecordOutBinaryFile(long position)
        {
            this.binReader.BaseStream.Position = position;
            this.binReader.ReadInt16();

            var record = new FileCabinetRecord()
            {
                Id = this.binReader.ReadInt32(),
                FirstName = Encoding.Unicode.GetString(this.binReader.ReadBytes(LengtOfString * 2)).Trim(),
                LastName = Encoding.Unicode.GetString(this.binReader.ReadBytes(LengtOfString * 2)).Trim(),
                DateOfBirth = DateTime.Parse($"{this.binReader.ReadInt32()}/{this.binReader.ReadInt32()}/{this.binReader.ReadInt32()}", CultureInfo.InvariantCulture),
                Expirience = this.binReader.ReadInt16(),
                Balance = this.binReader.ReadDecimal(),
                Nationality = Encoding.Unicode.GetString(this.binReader.ReadBytes(sizeof(char))).First(),
            };

            return record;
        }

        private List<FileCabinetRecord> GetRecordsCollection()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            for (int i = 0; i < this.position; i += RecordSize)
            {
                var record = this.ReadRecordOutBinaryFile(i);
                records.Add(record);
            }

            return records;
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

                    int size = this.realIdRecord.Count + this.removeIdRecords.Count;
                    var data = new RecordData();
                    data.firstName = record.FirstName;
                    data.lastName = record.LastName;
                    data.dateOfBirth = record.DateOfBirth;
                    data.balance = record.Balance;
                    data.expirience = record.Expirience;
                    data.nationality = record.Nationality;

                    if (this.realIdRecord.Contains(id))
                    {
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.WriteRecordToBinaryFile(RecordSize * size++, data, id);
                        this.position += RecordSize;
                        this.realIdRecord.Add(id);
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
    }
}
