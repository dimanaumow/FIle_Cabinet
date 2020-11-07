using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using FileCabinetApp.Service;

namespace FileCabinetGenerator
{
    public static class Program
    {
        public const string exportTypeCsv = "csv";
        public const string exportTypeXml = "xml";

        private static string outputType;
        private static string outputPath;
        private static int recordsAmount; 
        private static int startId;

        static void Main(string[] args)
        {
            var commandPairs = GetCurrentComandPairs(args);
            Handle(commandPairs);
            Console.WriteLine(outputType);
            Console.WriteLine(outputPath);
            Console.WriteLine(recordsAmount);
            Console.WriteLine(startId);

            foreach (var record in GenerateRecords())
            {
                Console.WriteLine(record);
            }
            Export();
        }

        private static IEnumerable<(string, string)> GetCurrentComandPairs(string[] args)
        {
            if (args is null)
            {
                yield break;
            }

            if (args.Length == 0)
            {
                yield break;
            }

            var parameters = string.Join(' ', args).Split(new char[] { ' ', '=' });

            for (int i = 0; i < parameters.Length; i+=2)
            {
                yield return (parameters[i], parameters[i + 1]);
            }
        }

        private static void Handle(IEnumerable<(string, string)> commandPairs)
        {
            foreach (var commandPair in commandPairs)
            {
                switch (commandPair.Item1)
                {
                    case CommandConstants.TYPE:
                    case CommandConstants.TYPESHORT:
                        outputType = commandPair.Item2;
                        break;
                    case CommandConstants.OUTPUT:
                    case CommandConstants.OUTPUTSHORT:
                        outputPath = commandPair.Item2;
                        break;
                    case CommandConstants.AMOUNT:
                    case CommandConstants.AMOUNTSHORT:
                        bool isDone = int.TryParse(commandPair.Item2, out recordsAmount);
                        if (!isDone)
                        {
                            throw new ArgumentException($"It's command {nameof(commandPair.Item2)} is incorrect");
                        }
                        break;
                    case CommandConstants.ID:
                    case CommandConstants.IDSHORT:
                        isDone = int.TryParse(commandPair.Item2, out startId);
                        if (!isDone)
                        {
                            throw new ArgumentException($"It's command {nameof(commandPair.Item2)} is incorrect");
                        }
                        break; 
                }
            }
        }

        private static IEnumerable<FileCabinetRecord> GenerateRecords()
        {
            var recordGenerator = new FileCabinetRecordGenerator();

            for (int i = 1; i <= recordsAmount; i++)
            { 
                yield return recordGenerator.Generate(startId++);
            }
        }

        private static void Export()
        {
            if (string.Equals(outputType, exportTypeCsv, StringComparison.OrdinalIgnoreCase))
            {
                ExportCsv();
            }
            else if (string.Equals(outputType, exportTypeXml, StringComparison.OrdinalIgnoreCase))
            {
                ExportXml();
            }
            else
            {
                Console.WriteLine($"This .{nameof(outputType)} type file is incorrect.");
            }
        }

        private static void ExportCsv()
        {
            using (var writer = new StreamWriter(outputPath))
            {
                var csvWriter = new RecordCsvWriter(writer);

                foreach (var record in GenerateRecords())
                {
                    csvWriter.Write(record);
                }
            }
        }

        private static void ExportXml()
        {
            var records = GenerateRecords();
            var collection = new List<SerializableRecord>();

            foreach (var record in records)
            {
                var serializeRecord = new SerializableRecord();
                serializeRecord.Id = record.Id;
                serializeRecord.FirstName = record.FirstName;
                serializeRecord.LastName = record.LastName;
                serializeRecord.dateOfBirth = record.DateOfBirth;
                serializeRecord.Epirience = record.Expirience;
                serializeRecord.Balance = record.Balance;
                serializeRecord.Nationality = record.Nationality;

                collection.Add(serializeRecord);
            }

            var serializableRecords = new SerializableCollection();
            serializableRecords.SerializeRecords = collection.ToArray();

            using (var writer = new StreamWriter(outputPath))
            {
                var xmlWriter = new RecordXmlWriter(XmlWriter.Create(writer), serializableRecords);
                xmlWriter.Write();
            }
        }
    }
}
