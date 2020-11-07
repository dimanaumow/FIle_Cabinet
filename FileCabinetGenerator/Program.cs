using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetGenerator
{
    public static class Program
    {
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

            var records = GenerateRecords();
            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
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
    }
}
