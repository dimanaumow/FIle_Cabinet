using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetGenerator
{
    public static class Program
    {
        private static string outputType;
        private static string outputPath;
        private static int recordsAmount; 
        private static int startId;

        private static Tuple<string, string>[] commands = new Tuple<string, string>[]
        {
            new Tuple<string, string>("--output-type", "-t"),
            new Tuple<string, string>("--output", "-o"),
            new Tuple<string, string>("--records-amount", "-a"),
            new Tuple<string, string>("--start-id", "-i"),
        };

        static void Main(string[] args)
        {
            var commandPairs = GetCurrentComandPairs(args);
            Handle(commandPairs);
            Console.WriteLine(outputType);
            Console.WriteLine(outputPath);
            Console.WriteLine(recordsAmount);
            Console.WriteLine(startId);
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
    }
}
