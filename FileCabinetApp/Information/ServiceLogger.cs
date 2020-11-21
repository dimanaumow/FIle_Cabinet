using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.Service;

#pragma warning disable CA1822
namespace FileCabinetApp.Information
{
    /// <summary>
    /// Service logger.
    /// </summary>
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly IFileCabinetService service;
        private readonly StreamWriter writer;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">The current service.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException($"{nameof(service)} cannot be null.");
            }

            this.service = service;

            string path = @"d:\AutocodeEPAM\FileCabinet\logData.txt";
            var stream = File.Exists(path) ? File.OpenWrite(path) : File.Create(path);
            this.writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Generates a unique user record.
        /// </summary>
        /// <param name="parameters">Parameters of new record.</param>
        /// <returns>Id of record.</returns>
        public int CreateRecord(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            int id = this.service.CreateRecord(parameters);
            this.WriteLogInFile(nameof(this.CreateRecord), this.GetInfoRecordData(parameters));
            this.WriteLogReturnInFile<int>(nameof(this.service.CreateRecord), id);
            return id;
        }

        /// <summary>
        /// Changes the record by given ID.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="parameters">Parameters of record.</param>
        public void EditRecord(int id, RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            this.service.EditRecord(id, parameters);
            this.WriteLogInFile(nameof(this.service.EditRecord), this.GetInfoRecordData(parameters));
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByAnd(WhereConditions conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException($"{nameof(conditions)} cannot be null.");
            }

            var collection = this.service.FindByAnd(conditions);
            this.WriteLogInFile(nameof(this.service.FindByAnd), conditions.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByAnd), conditions.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByOr(WhereConditions conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException($"{nameof(conditions)} cannot be null.");
            }

            var collection = this.service.FindByOr(conditions);
            this.WriteLogInFile(nameof(this.service.FindByOr), conditions.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByOr), conditions.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given firstName.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var collection = this.service.FindByFirstName(firstName);
            this.WriteLogInFile(nameof(this.service.FindByFirstName), firstName);
            this.WriteLogReturnInFile(nameof(this.service.FindByFirstName), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given lastName.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var collection = this.service.FindByLastName(lastName);
            this.WriteLogInFile(nameof(this.service.FindByLastName), lastName);
            this.WriteLogReturnInFile(nameof(this.service.FindByLastName), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var collection = this.service.FindByDateOfBirth(dateOfBirth);
            this.WriteLogInFile(nameof(this.service.FindByDateOfBirth), dateOfBirth);
            this.WriteLogReturnInFile(nameof(this.service.FindByDateOfBirth), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given experience.
        /// </summary>
        /// <param name="expirience">The user's experience.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience)
        {
            var collection = this.service.FindByExpirience(expirience);
            this.WriteLogInFile(nameof(this.service.FindByExpirience), expirience);
            this.WriteLogReturnInFile(nameof(this.service.FindByExpirience), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given balance.
        /// </summary>
        /// <param name="balance">The user's balance.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByBalance(string balance)
        {
            var collection = this.service.FindByBalance(balance);
            this.WriteLogInFile(nameof(this.service.FindByBalance), balance);
            this.WriteLogReturnInFile(nameof(this.service.FindByBalance), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given english level.
        /// </summary>
        /// <param name="englishLevel">The user's english level.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel)
        {
            var collection = this.service.FindByEnglishLevel(englishLevel);
            this.WriteLogInFile(nameof(this.service.FindByEnglishLevel), englishLevel);
            this.WriteLogReturnInFile(nameof(this.service.FindByEnglishLevel), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">The id of removed record.</param>
        /// <returns>Is removed record.</returns>
        public bool Remove(int id)
        {
            bool isRemoved = this.service.Remove(id);
            this.WriteLogInFile(nameof(this.service.Remove), id.ToString(CultureInfo.InvariantCulture));
            this.WriteLogReturnInFile(nameof(this.service.Remove), isRemoved.ToString());
            return isRemoved;
        }

        /// <summary>
        /// Deleted all removed record from file.
        /// </summary>
        public void Purge()
        {
            this.service.Purge();
            this.WriteLogInFile(nameof(this.service.Purge), string.Empty);
        }

        /// <summary>
        /// Give all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            var collection = this.service.GetRecords();
            this.WriteLogInFile(nameof(this.service.GetRecords), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetRecords), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Give the count of records.
        /// </summary>
        /// <returns>The count of records.</returns>
        public (int active, int removed) GetStat()
        {
            var stat = this.service.GetStat();
            this.WriteLogInFile(nameof(this.service.GetStat), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetStat), stat.ToString());
            return stat;
        }

        /// <summary>
        /// Make snapshot of the current service.
        /// </summary>
        /// <returns>Snapshot of the current service.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            var snapshot = this.service.MakeSnapShot();
            this.WriteLogInFile(nameof(this.service.MakeSnapShot), string.Empty);
            return snapshot;
        }

        /// <summary>
        /// Recovers saved snapshot recordings.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <returns>Count of recorves record.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int count = this.service.Restore(snapshot);
            this.WriteLogInFile(nameof(this.service.Restore), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.Restore), count.ToString(CultureInfo.InvariantCulture));
            return count;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.writer.Dispose();
            }

            this.disposed = true;
        }

        private void WriteLogInFile(string methodName, string info)
        {
            this.writer.WriteLine($"{DateTime.UtcNow} - Calling {methodName}() with {info}");
            this.writer.Flush();
        }

        private void WriteLogReturnInFile<T>(string methodName, T value)
        {
            this.writer.WriteLine($"{DateTime.UtcNow} - {methodName} return {value}");
            this.writer.Flush();
        }

        private string GetInfoRecordData(RecordData parameters)
            => $"FirstName = '{parameters.FirstName}', LastName = '{parameters.LastName}', " +
                $"DateOfBirth = '{parameters.DateOfBirth}', Experience = '{parameters.Experience}', " +
                $"Balance = '{parameters.Balance}', EnglishLevel = '{parameters.EnglishLevel}'.";
    }
}
