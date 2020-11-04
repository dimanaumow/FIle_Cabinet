using FileCabinetApp.Validators;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        public const int LengtOfString = 120;
        public const int SizeRecord = sizeof(char) + sizeof(int) + (2 * LengtOfString) + (3 * sizeof(int)) + sizeof(decimal) + sizeof(char) + sizeof(short);
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
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
            this.id = 1;
        }

        public int CreateRecord(RecordData parameters)
        {
            this.validator.ValidatePararmeters(parameters);

            var data = this.ParseRecordToByteArray(parameters, this.id);
            this.id++;
            this.fileStream.Write(data, 0, data.Length);
            this.fileStream.Flush();

            return this.id;
        }

        public void EditRecord(int id, RecordData parameters)
        {
            this.validator.ValidatePararmeters(parameters);

            var data = this.ParseRecordToByteArray(parameters, this.id);
            int startPosition = this.id + SizeRecord;
            this.fileStream.Position = startPosition;
            this.fileStream.Write(data, 0, data.Length);

            this.fileStream.Flush();
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
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            throw new NotImplementedException();
        }

        private byte[] ParseRecordToByteArray(RecordData parameters, int id)
        {
            short reserved = 0;
            byte[] data = new byte[SizeRecord];

            using (var memoryStream = new MemoryStream(data))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(reserved);
                binaryWriter.Write(id);

                var firstNameBytes = Encoding.ASCII.GetBytes(parameters.firstName);
                var lastNameBytes = Encoding.ASCII.GetBytes(parameters.lastName);

                var nameBuffer = new byte[LengtOfString];

                Array.Copy(firstNameBytes, nameBuffer, firstNameBytes.Length);
                binaryWriter.Write(nameBuffer, 0, nameBuffer.Length);

                Array.Copy(lastNameBytes, nameBuffer, lastNameBytes.Length);
                binaryWriter.Write(nameBuffer, 0, nameBuffer.Length);

                binaryWriter.Write(parameters.dateOfBirth.Year);
                binaryWriter.Write(parameters.dateOfBirth.Month);
                binaryWriter.Write(parameters.dateOfBirth.Day);
                binaryWriter.Write(parameters.balance);
                binaryWriter.Write(parameters.expirience);
                binaryWriter.Write(parameters.nationality);
            }

            return data;
        }

        private FileCabinetRecord MakeFileCabinetRecord(byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException($"{nameof(data)} cannot be null.");
            }

            var record = new FileCabinetRecord();
            using (var memoryStream = new MemoryStream(data))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                record.Id = binaryReader.ReadInt32();

                var nameLength = binaryReader.ReadInt32();
                var nameBuffer = binaryReader.ReadBytes(LengtOfString);

                record.FirstName = Encoding.ASCII.GetString(nameBuffer, 0, nameLength);

                var lastNameLength = binaryReader.ReadInt32();
                var lastNameBuffer = binaryReader.ReadBytes(LengtOfString);

                record.LastName = Encoding.ASCII.GetString(lastNameBuffer, 0, lastNameLength);

                int year = binaryReader.ReadInt32();
                int month = binaryReader.ReadInt32();
                int day = binaryReader.ReadInt32();

                record.DateOfBirth = new DateTime(year, month, day);
                record.Balance = binaryReader.ReadDecimal();
                record.Expirience = binaryReader.ReadInt16();
                record.Nationality = binaryReader.ReadChar();
            }

            return record;
        }

        private List<FileCabinetRecord> BinaryRecordsToList()
        {
            List<FileCabinetRecord> list = new List<FileCabinetRecord>();
            long numberOfRecords = this.fileStream.Length / SizeOfRecord;
            for (int i = 0; i < numberOfRecords; i++)
            {
                var recordBuffer = new byte[SizeOfRecord];

                this.fileStream.Read(recordBuffer, (i * (int)SizeOfRecord) + 1, (int)SizeOfRecord);
                var record = BytesToUser(recordBuffer);

                list.Add(record);
            }

            return list;
        }
    }
}
