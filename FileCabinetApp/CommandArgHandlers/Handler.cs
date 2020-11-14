using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandArgHandlers
{
    public static class Handler
    {
        public static IFileCabinetService Handle(string[] args)
        {
            var commandPairs = GetCurrentComandPairs(args);
            return HandleCommand(commandPairs);
        }

        private static IFileCabinetService HandleCommand(IEnumerable<(string, string)> commandPairs)
        {
            IFileCabinetService service = new FileCabinetMemoryService();
            IRecordValidator validator;

            foreach (var commandPair in commandPairs)
            {
                switch (commandPair.Item1)
                {
                
                }
            }

            return service;
        }

        public static IEnumerable<(string, string)> GetCurrentComandPairs(string[] args)
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

            int addition = 2;
            for (int i = 0; i < parameters.Length; i += addition)
            {
                if (parameters[i] == CommandArgConstant.singleComand[0] || parameters[i] == CommandArgConstant.singleComand[1])
                {
                    addition = 1;
                    yield return (parameters[i], string.Empty);
                }
                else
                {
                    addition = 2;
                    yield return (parameters[i], parameters[i + 1]);
                }
            }
        }
    }
}
