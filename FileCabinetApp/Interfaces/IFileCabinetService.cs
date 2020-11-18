using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Provide interface service for work with user's record.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Generates a unique user record.
        /// </summary>
        /// <param name="parameters">Parameters of new record.</param>
        /// <returns>Id of record.</returns>
        public int CreateRecord(RecordData parameters);

        /// <summary>
        /// Changes the record by given ID.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="parameters">Parameters of record.</param>
        public void EditRecord(int id, RecordData parameters);

        /// <summary>
        /// Find all records with given firstName.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The array of finded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Find all records with given lastName.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The array of finded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Find all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The array of finded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience);

        public IEnumerable<FileCabinetRecord> FindByBalance(string balance);

        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel);

        public bool Remove(int id);

        public void Purge();

        /// <summary>
        /// Give all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Give the count of records.
        /// </summary>
        /// <returns>The count of records.</returns>
        public (int active, int removed) GetStat();

        public FileCabinetServiceSnapshot MakeSnapShot();

        public int Restore(FileCabinetServiceSnapshot snapshot);
    }
}
