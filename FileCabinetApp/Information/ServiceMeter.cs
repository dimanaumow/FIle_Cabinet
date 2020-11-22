using System;
using System.Collections.Generic;
using System.Diagnostics;
using FileCabinetApp.Service;

#pragma warning disable CA1822
namespace FileCabinetApp.Information
{
    /// <summary>
    /// Sevice meter.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly Stopwatch stopwatch;
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">The current service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException($"{nameof(service)} cannot be null.");
            }

            this.stopwatch = new Stopwatch();
            this.service = service;
        }

        /// <summary>
        /// Generates a unique user record.
        /// </summary>
        /// <param name="parameters">Parameters of new record.</param>
        /// <returns>Id of record.</returns>
        public int CreateRecord(RecordData parameters)
        {
            this.stopwatch.Restart();
            var id = this.service.CreateRecord(parameters);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.CreateRecord), this.stopwatch.ElapsedTicks);
            return id;
        }

        /// <summary>
        /// Changes the record by given ID.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="parameters">Parameters of record.</param>
        public void EditRecord(int id, RecordData parameters)
        {
            this.stopwatch.Restart();
            this.service.EditRecord(id, parameters);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.EditRecord), this.stopwatch.ElapsedTicks);
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

            this.stopwatch.Restart();
            var collection = this.service.FindByAnd(conditions);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByAnd), this.stopwatch.ElapsedTicks);
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

            this.stopwatch.Restart();
            var collection = this.service.FindByOr(conditions);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByOr), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Find all records with given firstName.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByFirstName(firstName);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByFirstName), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Find all records with given lastName.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByLastName(lastName);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByLastName), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Find all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByDateOfBirth(dateOfBirth);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByDateOfBirth), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Find all records with given experience.
        /// </summary>
        /// <param name="expirience">The user's experience.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByExpirience(string expirience)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByExpirience(expirience);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByExpirience), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Find all records with given balance.
        /// </summary>
        /// <param name="balance">The user's balance.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByBalance(string balance)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByBalance(balance);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByBalance), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Find all records with given english level.
        /// </summary>
        /// <param name="englishLevel">The user's english level.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByEnglishLevel(string englishLevel)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByEnglishLevel(englishLevel);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByEnglishLevel), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">The id of removed record.</param>
        /// <returns>Is removed record.</returns>
        public bool Remove(int id)
        {
            this.stopwatch.Restart();
            var isRemove = this.service.Remove(id);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.Remove), this.stopwatch.ElapsedTicks);
            return isRemove;
        }

        /// <summary>
        /// Deleted all removed record from file.
        /// </summary>
        public void Purge()
        {
            this.stopwatch.Restart();
            this.service.Purge();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.Purge), this.stopwatch.ElapsedTicks);
        }

        /// <summary>
        /// Give all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Restart();
            var collection = this.service.GetRecords();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.GetRecords), this.stopwatch.ElapsedTicks);
            return collection;
        }

        /// <summary>
        /// Give the count of records.
        /// </summary>
        /// <returns>The count of records.</returns>
        public (int active, int removed) GetStat()
        {
            this.stopwatch.Restart();
            var stat = this.service.GetStat();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.GetStat), this.stopwatch.ElapsedTicks);
            return stat;
        }

        /// <summary>
        /// Make snapshot of the current service.
        /// </summary>
        /// <returns>Snapshot of the current service.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            this.stopwatch.Restart();
            var snapshot = this.service.MakeSnapShot();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.MakeSnapShot), this.stopwatch.ElapsedTicks);
            return snapshot;
        }

        /// <summary>
        /// Recovers saved snapshot recordings.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch.Restart();
            this.service.Restore(snapshot);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.Restore), this.stopwatch.ElapsedTicks);
        }

        private void Information(string methodName, long ticks)
            => Console.WriteLine($"{methodName} method execution duration is {ticks} ticks.");
    }
}
