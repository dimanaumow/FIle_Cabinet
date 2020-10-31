using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;
        private XmlSerializer serializer;
        private SerializableCollection collection;
        private FileCabinetRecord[] records;

        public FileCabinetRecordXmlWriter(XmlWriter writer, FileCabinetRecord[] records)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.writer = writer;
            this.collection = new SerializableCollection();
            this.serializer = new XmlSerializer(typeof(SerializableCollection));
            this.records = records;
        }

        public void Write()
        {
            var listRecords = new List<FileCabinetRecord>(this.records);
            foreach (var record in listRecords)
            {
                this.collection.Records.Add(record);
            }

            this.serializer.Serialize(this.writer, this.collection);
        }
    }
}
