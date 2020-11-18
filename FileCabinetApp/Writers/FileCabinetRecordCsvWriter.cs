using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// The csv writer.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The csv writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            this.writer = writer;
        }

        /// <summary>
        /// Write records in csv file.
        /// </summary>
        /// <param name="record">Record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} cannot be null.");
            }

            this.writer.WriteLine(record.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
