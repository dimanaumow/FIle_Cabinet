using System;
using System.Collections.Generic;
using FileCabinetApp.Service;

namespace FileCabinetApp.Comparers
{
    /// <summary>
    /// Compare records.
    /// </summary>
    public class RecordComparer : IComparer<FileCabinetRecord>
    {
        /// <summary>
        /// Compare the records by Id.
        /// </summary>
        /// <param name="lhs">Lhs record.</param>
        /// <param name="rhs">Rhs record.</param>
        /// <returns>Compare int.</returns>
        /// <exception cref="ArgumentNullException">Throws, when lsh or rhs is null.</exception>
        public int Compare(FileCabinetRecord lhs, FileCabinetRecord rhs)
        {
            if (lhs is null)
            {
                throw new ArgumentNullException($"{nameof(lhs)} cannot be null.");
            }

            if (rhs is null)
            {
                throw new ArgumentNullException($"{nameof(rhs)} cannot be null.");
            }

            return lhs.Id - rhs.Id;
        }
    }
}
