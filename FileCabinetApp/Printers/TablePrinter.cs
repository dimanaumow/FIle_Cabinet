using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Service;

#pragma warning disable CA1822
namespace FileCabinetApp.Printers
{
    /// <summary>
    /// Table printer.
    /// </summary>
    public class TablePrinter
    {
        private const char Plus = '+';
        private const char VerticalBorder = '|';
        private const char HorizontalBorder = '-';
        private const char WhiteSpace = ' ';

        private Dictionary<string, int> maxCeilLengths;

        /// <summary>
        /// Print record in table format.
        /// </summary>
        /// <param name="records">The records collection.</param>
        /// <param name="property">The property to print.</param>
        public void Print(IEnumerable<FileCabinetRecord> records, List<string> property)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null");
            }

            if (property is null)
            {
                throw new ArgumentNullException($"{nameof(property)} cannot be null.");
            }

            this.maxCeilLengths = this.GetMaxCeilLengths(records);

            this.PrintBorder(property);
            this.PrintHeader(property);
            this.PrintBorder(property);

            foreach (var item in records)
            {
                this.PrintContent(item, property);
                this.PrintBorder(property);
            }
        }

        private void PrintBorder(List<string> properties)
        {
            var sb = new StringBuilder();
            var recordProperties = typeof(FileCabinetRecord).GetProperties();

            foreach (var prop in recordProperties)
            {
                if (properties.FindIndex(x => x.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    string value = new string(HorizontalBorder, this.maxCeilLengths[prop.Name]);

                    if (prop.PropertyType.IsValueType)
                    {
                        sb.Append($"{Plus}{value.PadRight(this.maxCeilLengths[prop.Name], HorizontalBorder)}");
                    }
                    else
                    {
                        sb.Append($"{Plus}{value.PadLeft(this.maxCeilLengths[prop.Name], HorizontalBorder)}");
                    }
                }
            }

            sb.Append($"{Plus}");
            Console.WriteLine(sb);
        }

        private void PrintHeader(List<string> properties)
        {
            var sb = new StringBuilder();
            var recordProperties = typeof(FileCabinetRecord).GetProperties();

            foreach (var prop in recordProperties)
            {
                if (properties == null || properties.FindIndex(x => x.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    if (prop.PropertyType.IsValueType)
                    {
                        sb.Append($"{VerticalBorder}{prop.Name.PadRight(this.maxCeilLengths[prop.Name], WhiteSpace)}");
                    }
                    else
                    {
                        sb.Append($"{VerticalBorder}{prop.Name.PadLeft(this.maxCeilLengths[prop.Name], WhiteSpace)}");
                    }
                }
            }

            sb.Append($"{VerticalBorder}");
            Console.WriteLine(sb);
        }

        private void PrintContent(FileCabinetRecord record, List<string> properties)
        {
            var sb = new StringBuilder();
            var recordProperties = typeof(FileCabinetRecord).GetProperties();

            foreach (var prop in recordProperties)
            {
                if (properties.FindIndex(x => x.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    string value;
                    if (!prop.PropertyType.Equals(typeof(DateTime)))
                    {
                        value = string.Format(CultureInfo.InvariantCulture, "{0:0}", prop.GetValue(record));
                    }
                    else
                    {
                        value = string.Format(CultureInfo.InvariantCulture, "{0:MM-dd-yyyy}", prop.GetValue(record));
                    }

                    if (prop.PropertyType.IsValueType)
                    {
                        sb.Append($"{VerticalBorder}{value.PadRight(this.maxCeilLengths[prop.Name], WhiteSpace)}");
                    }
                    else
                    {
                        sb.Append($"{VerticalBorder}{value.PadLeft(this.maxCeilLengths[prop.Name], WhiteSpace)}");
                    }
                }
            }

            sb.Append($"{VerticalBorder}");
            Console.WriteLine(sb);
        }

        private Dictionary<string, int> GetMaxCeilLengths(IEnumerable<FileCabinetRecord> records)
        {
            var propertyLengthPairs = new Dictionary<string, int>();

            if (!records.Any())
            {
                return propertyLengthPairs;
            }

            var recordProperties = typeof(FileCabinetRecord).GetProperties();

            foreach (var propertie in recordProperties)
            {
                string itemStringView;

                int max = records.Max(x =>
                {
                    object value = propertie.GetValue(x);
                    Type itemType = propertie.PropertyType;

                    if (itemType.Equals(typeof(DateTime)))
                    {
                        itemStringView = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MMM-dd}", value);
                    }
                    else
                    {
                        itemStringView = value.ToString();
                    }

                    return value is null ? 0 : itemStringView.Length + 1;
                });

                propertyLengthPairs.Add(propertie.Name, Math.Max(max, propertie.Name.Length));
            }

            return propertyLengthPairs;
        }
    }
}
