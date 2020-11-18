using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Serializable record data.
    /// </summary>
    [Serializable]
    public class SerializableRecord
    {
        /// <summary>
        /// Date of birth.
        /// </summary>
        public DateTime dateOfBirth;

        /// <summary>
        /// Gets or sets records id.
        /// </summary>
        /// <value>The records id.</value>
        [XmlAttribute("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets records firstName.
        /// </summary>
        /// <value>The records firstName.</value>
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets record lastName.
        /// </summary>
        /// <value>The records lastName.</value>
        [XmlElement("LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets records experience.
        /// </summary>
        /// <value>The records experience.</value>
        [XmlElement("Experience")]
        public short Eperience { get; set; }

        /// <summary>
        /// Gets or sets records balance.
        /// </summary>
        /// <value>The records balance.</value>
        [XmlElement("Balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets records english level.
        /// </summary>
        /// <value>The records english level.</value>
        [XmlElement("EnglishLevel")]
        public char EnglishLevel { get; set; }

        /// <summary>
        /// Gets records date of birth.
        /// </summary>
        /// <value>The records date of birth.</value>
        [XmlElement("DateOfBirth")]
        public string DateOfBirth
        {
            get => this.dateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
