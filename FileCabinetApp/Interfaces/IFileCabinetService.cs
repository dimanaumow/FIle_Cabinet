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
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByAnd(WhereConditions conditions);

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByOr(WhereConditions conditions);

        /// <summary>
        /// Find all records with given firstName.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Find all records with given lastName.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Find all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Find all records with given experience.
        /// </summary>
        /// <param name="expirience">The user's experience.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience);

        /// <summary>
        /// Find all records with given balance.
        /// </summary>
        /// <param name="balance">The user's balance.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByBalance(string balance);

        /// <summary>
        /// Find all records with given english level.
        /// </summary>
        /// <param name="englishLevel">The user's english level.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel);

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">The id of removed record.</param>
        /// <returns>Is removed record.</returns>
        public bool Remove(int id);

        /// <summary>
        /// Deleted all removed record from file.
        /// </summary>
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

        /// <summary>
        /// Make snapshot of the current service.
        /// </summary>
        /// <returns>Snapshot of the current service.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot();

        /// <summary>
        /// Recovers saved snapshot recordings.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <returns>Count of recorves record.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot);
    }
}
