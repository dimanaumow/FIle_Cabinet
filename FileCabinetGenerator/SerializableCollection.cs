using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    [XmlRoot("Records")]
    public class SerializableCollection
    {
        [XmlElement("Record")]
        public SerializableRecord[] SerializeRecords { get; set; }
    }
}
