using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Information;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandArgHandlers
{
    public class Handler
    {
        public const string FileServic = "file";
        public const string MemoryServic = "memory";

        public const string Default = "default";
        public const string Custom = "custom";

        public const string fullPath = "cabinet-records.db";

        public IFileCabinetService decoratedService;
        public IFileCabinetService service = new FileCabinetMemoryService();
        public IRecordValidator validator = new ValidatorBuilder().Create(Default);

        public void Handle(string[] args)
        {
            var commandPairs = this.GetCurrentComandPairs(args);
            this.HandleCommand(commandPairs);
        }

        public IFileCabinetService GetService()
        {
            if (this.decoratedService is null)
            {
                return this.service;
            }

            return this.decoratedService;
        }

        private void HandleCommand(IEnumerable<(string, string)> commandPairs)
        {
            foreach (var commandPair in commandPairs)
            {
                switch (commandPair.Item1)
                {
                    case CommandArgConstant.STORAGE:
                    case CommandArgConstant.STORAGESHORT:
                        this.SetStorage(commandPair.Item2);
                        break;
                    case CommandArgConstant.VALIDATIONRULE:
                    case CommandArgConstant.VALIDATIONRULESHORT:
                        this.SetValidator(commandPair.Item2);
                        break;
                    case CommandArgConstant.STOPWATCH:
                        this.decoratedService = new ServiceMeter(this.service);
                        break;
                    case CommandArgConstant.LOGGER:
                        this.decoratedService = new ServiceLogger(this.service);
                        break;
                }
            }
        }

        private void SetStorage(string storageType)
        {
            if (string.Equals(storageType, FileServic, StringComparison.OrdinalIgnoreCase))
            {
                FileStream fileStream = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                this.service = new FileCabinetFilesystemService(this.validator, fileStream);
            }

            if (string.Equals(storageType, MemoryServic, StringComparison.OrdinalIgnoreCase))
            {
                this.service = new FileCabinetMemoryService(this.validator);
            }
        }

        private void SetValidator(string rule)
        {
            if (string.Equals(rule, Default, StringComparison.OrdinalIgnoreCase))
            {
                this.validator = new ValidatorBuilder().Create(Default);
            }

            if (string.Equals(rule, Custom, StringComparison.OrdinalIgnoreCase))
            {
                this.validator = new ValidatorBuilder().Create(Custom);
            }
        }

        private IEnumerable<(string, string)> GetCurrentComandPairs(string[] args)
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
