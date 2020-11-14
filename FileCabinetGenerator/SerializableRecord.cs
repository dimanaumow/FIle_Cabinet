using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    [Serializable]
    public class SerializableRecord
    {
        public DateTime dateOfBirth;

        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("Experience")]
        public short Experience { get; set; }

        [XmlElement("Balance")]
        public decimal Balance { get; set; }

        [XmlElement("EnglishLevel")]
        public char EnglishLevel { get; set; }

        [XmlElement("DateOfBirth")]
        public string DateOfBirth
        {
            get => this.dateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
