using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Conditions for improved find.
    /// </summary>
    public class WhereConditions
    {
        /// <summary>
        /// Gets or sets condition firstName.
        /// </summary>
        /// <value>FirstName.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets condition lastName.
        /// </summary>
        /// <value>LastName.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets condition nullable dateOfBirth.
        /// </summary>
        /// <value>Nullable date.</value>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets condition nullable expirience.
        /// </summary>
        /// <value>Nullable expirience.</value>
        public short? Experience { get; set; }

        /// <summary>
        /// Gets or sets condition nullable balance.
        /// </summary>
        /// <value>Nullable balance.</value>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Gets or sets condition nullable english level.
        /// </summary>
        /// <value>Nullable english level.</value>
        public char? EnglishLevel { get; set; }
    }
}
