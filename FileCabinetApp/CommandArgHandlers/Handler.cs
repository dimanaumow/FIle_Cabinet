using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Information;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

#pragma warning disable CA1822
namespace FileCabinetApp.CommandArgHandlers
{
    /// <summary>
    /// Handle command arguments.
    /// </summary>
    public class Handler
    {
        private const string FileServic = "file";
        private const string MemoryServic = "memory";

        private const string Default = "default";
        private const string Custom = "custom";

        private const string FullPath = "cabinet-records.db";

        private IFileCabinetService decoratedService;
        private IFileCabinetService service = new FileCabinetMemoryService();
        private IRecordValidator validator = new ValidatorBuilder().Create(Default);

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler"/> class.
        /// </summary>
        /// <param name="args">The command args.</param>
        public void Handle(string[] args)
        {
            var commandPairs = this.GetCurrentComandPairs(args);
            this.HandleCommand(commandPairs);
        }

        /// <summary>
        /// Gets fileCabinet service.
        /// </summary>
        /// <returns>The fileCabinetService.</returns>
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
                FileStream fileStream = File.Open(FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
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

        /// <summary>
        /// Return command pair: (command, value).
        /// </summary>
        /// <param name="args">Command args.</param>
        /// <returns>The command pair (command, value).</returns>
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
                if (parameters[i] == CommandArgConstant.SingleComand[0] || parameters[i] == CommandArgConstant.SingleComand[1])
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
