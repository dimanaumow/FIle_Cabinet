using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Serializeble record array.
    /// </summary>
    [XmlRoot("Records")]
    public class SerializableRecordsArray
    {
        /// <summary>
        /// Gets or sets serializable record.
        /// </summary>
        /// <value>Serializable record.</value>
        [XmlElement("Record")]
#pragma warning disable CA1819
        public SerializableRecord[] SerializeRecords { get; set; }
#pragma warning restore CA1819
    }
}