using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Encapsulates user data as a record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets the user's id.
        /// </summary>
        /// <value>User id.</value>
        public int Id { get; set; }

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
        public short Expirience { get; set; }

        /// <summary>
        /// Gets or sets the user's balance.
        /// </summary>
        /// <value>User balance.</value>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the user's nationality.
        /// </summary>
        /// <value>User nationality.</value>
        public char Nationality { get; set; }
    }
}
