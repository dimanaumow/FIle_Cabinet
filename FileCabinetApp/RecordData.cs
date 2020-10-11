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
        public string firstName;
        public string lastName;
        public DateTime dateOfBirth;
        public short expirience;
        public decimal balance;
        public char nationality;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordData"/> class.
        /// </summary>
        public RecordData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordData"/> class.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <param name="lastName">User lastName.</param>
        /// <param name="dateOfBirth">User's date of birth.</param>
        /// <param name="expirience">User's expirience.</param>
        /// <param name="balance">User's balance.</param>
        /// <param name="nationality">User's nationality.</param>
        public RecordData(string firstName, string lastName, DateTime dateOfBirth, short expirience, decimal balance, char nationality)
            : this()
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.dateOfBirth = dateOfBirth;
            this.expirience = expirience;
            this.balance = balance;
            this.nationality = nationality;
        }
    }
}
