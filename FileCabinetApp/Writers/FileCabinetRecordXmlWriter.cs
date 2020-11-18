using System;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Xml writer.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlSerializer serializer;
        private readonly XmlWriter writer;
        private readonly SerializableRecordsArray records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        /// <param name="records">The serializable records.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer, SerializableRecordsArray records)
        {
            this.writer = writer;
            this.records = records;
            this.serializer = new XmlSerializer(typeof(SerializableRecordsArray));
        }

        /// <summary>
        /// Write record in xml format.
        /// </summary>
        public void Write()
        {
            this.serializer.Serialize(this.writer, this.records);
        }
    }
}
