using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records; 

        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.records = records;
        }

        public void SaveToCSV(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            TextWriter textWriter = writer;

            var csvWriter = new FileCabinetRecordCsvWriter(textWriter);
            textWriter.Write("Id,First Name,Last Name,Date of Birth");

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }
    }
}
