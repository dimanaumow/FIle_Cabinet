using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace FileCabinetGenerator
{
    public class RecordXmlWriter
    {
        private readonly XmlSerializer serializer;
        private readonly XmlWriter writer;
        private readonly SerializableCollection records;

        public RecordXmlWriter(XmlWriter writer, SerializableCollection records)
        {
            this.writer = writer;
            this.records = records;
            this.serializer = new XmlSerializer(typeof(SerializableCollection));
        }

        public void Write()
        {
            this.serializer.Serialize(writer, records);
        }
    }
}
