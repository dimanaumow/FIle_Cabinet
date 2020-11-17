using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandPromt
{
    public static class CommandPromtHandler
    {
        private const int SubtokenLength = 2;
        private const double ThresholdWord = 0.5d;

        private const string are = "are";
        private const string @is = "is";
        private static string[] commands =
        {
            "find",
            "update",
            "exit",
            "help",
            "delete",
            "list",
            "import",
            "purge",
            "stat",
            "insert",
            "export",
            "create",
        };

        public static void GetTheMostSimular(string incorrect)
        {
            var sb = new StringBuilder();
            int count = 0;
            foreach (var command in commands)
            {
                if (IsSimular(command, incorrect))
                {
                    sb.Append($"{Environment.NewLine}{command}");
                    count++;
                }
            }

            if (count == 0)
            {
                return;
            }

            if (count == 1)
            {
                Console.WriteLine($"The most simmular command {are}: {sb}");
            }
            else
            {
                Console.WriteLine($"The most simmular command {@is}: {sb}");
            }
        }

        private static bool IsSimular(string firstToken, string secondToken)
        {
            var equalSubtokensCount = 0;
            var usedTokens = new bool[secondToken.Length - SubtokenLength + 1];
            for (var i = 0; i < firstToken.Length - SubtokenLength + 1; ++i)
            {
                var subtokenFirst = firstToken.Substring(i, SubtokenLength);
                for (var j = 0; j < secondToken.Length - SubtokenLength + 1; ++j)
                {
                    if (!usedTokens[j])
                    {
                        var subtokenSecond = secondToken.Substring(j, SubtokenLength);
                        if (subtokenFirst.Equals(subtokenSecond))
                        {
                            equalSubtokensCount++;
                            usedTokens[j] = true;
                            break;
                        }
                    }
                }
            }

            var subtokenFirstCount = firstToken.Length - SubtokenLength + 1;
            var subtokenSecondCount = secondToken.Length - SubtokenLength + 1;

            var tanimoto = (1.0 * equalSubtokensCount) / (subtokenFirstCount + subtokenSecondCount - equalSubtokensCount);

            return ThresholdWord <= tanimoto;
        }

        //две первые буквы совпадают
        //длина одинаковая и совпадают три буквы на одинаковых позициях

    }
}
