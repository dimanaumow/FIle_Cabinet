using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Saved statement for FileCabinetService.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The array of records.</param>
        /// <exception cref="ArgumentNullException">Throws, when records is null.</exception>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.records = records;
            this.NotImported = new List<string>();
        }

        /// <summary>
        /// Gets list of not imorted records.
        /// </summary>
        /// <value>Count of imported records.</value>
        public IList<string> NotImported { get; }

        /// <summary>
        /// Gets record collection.
        /// </summary>
        /// <value>The record collection.</value>
        public IReadOnlyCollection<FileCabinetRecord> Records => this.records;

        /// <summary>
        /// Save records array to csv file.
        /// </summary>
        /// <param name="writer">The csv writer.</param>
        public void SaveToCSV(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            var csvWriter = new FileCabinetRecordCsvWriter(writer);

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        /// Save records array to xml file.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        public void SaveToXml(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            var collection = new List<SerializableRecord>();

            foreach (var record in this.records)
            {
                var serializeRecord = new SerializableRecord();
                serializeRecord.Id = record.Id;
                serializeRecord.FirstName = record.FirstName;
                serializeRecord.LastName = record.LastName;
                serializeRecord.DateOfBirth = record.DateOfBirth;
                serializeRecord.Eperience = record.Experience;
                serializeRecord.Balance = record.Balance;
                serializeRecord.EnglishLevel = record.EnglishLevel;

                collection.Add(serializeRecord);
            }

            var serializableRecords = new SerializableRecordsArray();
            serializableRecords.SerializeRecords = collection.ToArray();

            var xmlWriter = new FileCabinetRecordXmlWriter(XmlWriter.Create(writer), serializableRecords);
            xmlWriter.Write();
        }

        /// <summary>
        /// Read the records from csv file.
        /// </summary>
        /// <param name="reader">The csv reader.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException($"{nameof(reader)} cannot be null.");
            }

            this.NotImported.Clear();
            using var csvReader = new FileCabinetRecordCsvReader(reader);
            this.records = csvReader.Read().ToArray();
        }

        /// <summary>
        /// Read the records from xml file.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        public void LoadFromXml(StreamReader reader)
        {
            this.NotImported.Clear();
            if (reader is null)
            {
                throw new ArgumentNullException($"{nameof(reader)} cannot be null.");
            }

            using var xmlReader = new FileCabinetRecordXmlReader(reader);
            this.records = xmlReader.Read().ToArray();
        }
    }
}
