using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Serializeble record array.
    /// </summary>
    [XmlRoot("Records")]
    public class SerializableRecordsArray
    {
        [XmlElement("Record")]
        public SerializableRecord[] SerializeRecords { get; set; }
    }
}