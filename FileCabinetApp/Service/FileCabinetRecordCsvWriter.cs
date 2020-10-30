using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            this.writer = writer;
        }

        public void Write(FileCabinetRecord record)
        {
            writer.Write(record.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
