using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    [XmlInclude(typeof(FileCabinetRecord))]
    public class SerializableCollection
    {
        public SerializableCollection()
        {
            this.Records = new List<FileCabinetRecord>();
        }

        [XmlArray("Records")]
        [XmlArrayItem("Record")]
        public List<FileCabinetRecord> Records { get; }
    }
}
