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
        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;
        private readonly IRecordValidator validator;
        private int position;
        private bool disposed;
        private int id;

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
        }

        public int CreateRecord(RecordData parameters)
        {
            this.validator.ValidatePararmeters(parameters);
            this.WriteRecordToBinaryFile(this.position, parameters, this.id);
            this.position += RecordSize;
            return this.id++;
        }

        public void EditRecord(int id, RecordData parameters)
        {
            if (id > this.id)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            int position = (id - 1) * RecordSize;
            this.WriteRecordToBinaryFile(position, parameters, id);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
            =>
            new ReadOnlyCollection<FileCabinetRecord>(this.GetRecordsCollection());

        public int GetStat()
            => this.GetRecordsCollection().Count;

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            throw new NotImplementedException();
        }

        private void WriteRecordToBinaryFile(int position, RecordData parameters, int id)
        {
            this.binWriter.Seek(position, SeekOrigin.Begin);
            this.binWriter.Write((short)0);
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
