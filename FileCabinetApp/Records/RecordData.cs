using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Encapsulted data of record.
    /// </summary>
    public class RecordData
    {
        /// <summary>
        /// Gets or sets the user's firstName.
        /// </summary>
        /// <value>User firstName.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's lastName.
        /// </summary>
        /// <value>User lastName.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// </summary>
        /// <value>User date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the user's work expirience.
        /// </summary>
        /// <value>User expirience.</value>
        public short Experience { get; set; }

        /// <summary>
        /// Gets or sets the user's balance.
        /// </summary>
        /// <value>User balance.</value>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the user's nationality.
        /// </summary>
        /// <value>User nationality.</value>
        public char EnglishLevel { get; set; }
    }
}
