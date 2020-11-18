using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Xml reader.
    /// </summary>
    public class FileCabinetRecordXmlReader : IDisposable
    {
        private readonly StreamReader streamReader;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="streamReader">The xml reader.</param>
        public FileCabinetRecordXmlReader(StreamReader streamReader)
        {
            if (streamReader is null)
            {
                throw new ArgumentNullException($"{nameof(streamReader)} cannot be null.");
            }

            this.streamReader = streamReader;
        }

        /// <summary>
        /// Read recards from xml file.
        /// </summary>
        /// <returns>The records collection.</returns>
        public ReadOnlyCollection<FileCabinetRecord> Read()
        {
            var readRecords = new List<FileCabinetRecord>();
            this.streamReader.BaseStream.Position = 0;

            using (var xmlReader = new XmlTextReader(this.streamReader))
            {
                var serializer = new XmlSerializer(typeof(SerializableRecordsArray));
                var serializableRecords = (SerializableRecordsArray)serializer.Deserialize(xmlReader);

                foreach (var serializableRecord in serializableRecords.SerializeRecords)
                {
                    readRecords.Add(this.BuildRecord(serializableRecord));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(readRecords);
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
                this.streamReader.Close();
            }

            this.disposed = true;
        }

        private FileCabinetRecord BuildRecord(SerializableRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} cannot be null.");
            }

            return new FileCabinetRecord
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                Experience = record.Eperience,
                Balance = record.Balance,
                EnglishLevel = record.EnglishLevel,
            };
        }
    }
}
