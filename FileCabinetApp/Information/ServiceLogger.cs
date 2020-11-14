using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.Information
{
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly IFileCabinetService service;
        private readonly StreamWriter writer;
        private bool disposed;

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

        public int CreateRecord(RecordData parameters)
        {
            int id = this.service.CreateRecord(parameters);
            this.WriteLogInFile(nameof(this.CreateRecord), this.GetInfoRecordData(parameters));
            this.WriteLogReturnInFile<int>(nameof(this.service.CreateRecord), id);
            return id;
        }

        public void EditRecord(int id, RecordData parameters)
        {
            this.service.EditRecord(id, parameters);
            this.WriteLogInFile(nameof(this.service.EditRecord), GetInfoRecordData(parameters));
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var collection = this.service.FindByFirstName(firstName);
            WriteLogInFile(nameof(this.service.FindByFirstName), firstName);
            WriteLogReturnInFile(nameof(this.service.FindByFirstName), collection.ToString());
            return collection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var collection = this.service.FindByLastName(lastName);
            WriteLogInFile(nameof(this.service.FindByLastName), lastName);
            WriteLogReturnInFile(nameof(this.service.FindByLastName), collection.ToString());
            return collection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var collection = this.service.FindByDateOfBirth(dateOfBirth);
            this.WriteLogInFile(nameof(this.service.FindByDateOfBirth), dateOfBirth);
            this.WriteLogReturnInFile(nameof(this.service.FindByDateOfBirth), collection.ToString());
            return collection;
        }

        public bool Remove(int id)
        {
            bool isRemoved = this.service.Remove(id);
            this.WriteLogInFile(nameof(this.service.Remove), id.ToString(CultureInfo.InvariantCulture));
            this.WriteLogReturnInFile(nameof(this.service.Remove), isRemoved.ToString());
            return isRemoved;
        }

        public void Purge()
        {
            this.service.Purge();
            this.WriteLogInFile(nameof(this.service.Purge), string.Empty);
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var collection = this.service.GetRecords();
            this.WriteLogInFile(nameof(this.service.GetRecords), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetRecords), collection.ToString());
            return collection;
        }

        public (int real, int removed) GetStat()
        {
            var stat = this.service.GetStat();
            this.WriteLogInFile(nameof(this.service.GetStat), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetStat), stat.ToString());
            return stat;
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            var snapshot = this.service.MakeSnapShot();
            this.WriteLogInFile(nameof(this.service.MakeSnapShot), string.Empty);
            return snapshot;
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int count = this.service.Restore(snapshot);
            this.WriteLogInFile(nameof(this.service.Restore), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.Restore), count.ToString(CultureInfo.InvariantCulture));
            return count; 
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
            => $"FirstName = '{parameters.firstName}', LastName = '{parameters.lastName}', " +
                $"DateOfBirth = '{parameters.dateOfBirth}', Experience = '{parameters.experience}', " +
                $"Balance = '{parameters.balance}', EnglishLevel = '{parameters.englishLevel}'.";
    }
}
