using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    public class FileCabinetRecordXmlReader : IDisposable
    {
        private readonly StreamReader streamReader;
        private bool disposed;

        public FileCabinetRecordXmlReader(StreamReader streamReader)
        {
            if (streamReader is null)
            {
                throw new ArgumentNullException($"{nameof(streamReader)} cannot be null.");
            }

            this.streamReader = streamReader;
        }

        public ReadOnlyCollection<FileCabinetRecord> Read()
        {
            var readRecords = new List<FileCabinetRecord>();
            this.streamReader.BaseStream.Position = 0;

            using (var xmlReader = new XmlTextReader(this.streamReader))
            {
                var serializer = new XmlSerializer(typeof(SerializableCollection));
                var serializableRecords = (SerializableCollection)serializer.Deserialize(xmlReader);

                foreach (var serializableRecord in serializableRecords.SerializeRecords)
                {
                    readRecords.Add(this.BuildRecord(serializableRecord));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(readRecords);
        }

        public FileCabinetRecord BuildRecord(SerializableRecord record)
            => new FileCabinetRecord
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.dateOfBirth,
                Experience = record.Epirience,
                Balance = record.Balance,
                EnglishLevel = record.Nationality,
            };

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
                this.streamReader.Close();
            }

            this.disposed = true;
        }
    }
}
