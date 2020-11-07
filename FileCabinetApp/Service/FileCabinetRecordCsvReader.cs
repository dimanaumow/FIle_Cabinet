using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetRecordCsvReader : IDisposable
    {
        private readonly StreamReader streamReader;
        private bool disposed;

        public FileCabinetRecordCsvReader(StreamReader streamReader)
        {
            if (streamReader is null)
            {
                throw new ArgumentNullException($"{nameof(streamReader)} cannot be null.");
            }

            this.streamReader = streamReader;
            this.disposed = true;
        }

        public ReadOnlyCollection<FileCabinetRecord> Read()
        {
            var readRecords = new List<FileCabinetRecord>();
            this.streamReader.BaseStream.Position = 0;

            while (!this.streamReader.EndOfStream)
            {
                var data = this.streamReader.ReadLine().Split(',');
                var record = this.BuildRecord(data);
                readRecords.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(readRecords);
        }

        private FileCabinetRecord BuildRecord(string[] data)
            => new FileCabinetRecord
            {
                Id = int.Parse(data[0].Substring(1).Trim(), CultureInfo.InvariantCulture),
                FirstName = data[1].Trim(),
                LastName = data[2].Trim(),
                DateOfBirth = DateTime.Parse(data[3].Trim(), CultureInfo.InvariantCulture),
                Expirience = short.Parse(data[4].Trim(), CultureInfo.InvariantCulture),
                Balance = decimal.Parse(data[5].Trim(), CultureInfo.InvariantCulture),
                Nationality = char.Parse(data[6].Trim()),
            };

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
                this.streamReader.Close();
            }

            this.disposed = true;
        }
    }
}
