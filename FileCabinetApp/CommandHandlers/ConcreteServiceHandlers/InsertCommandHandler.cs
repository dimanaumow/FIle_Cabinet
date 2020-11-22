using System;
using System.ComponentModel;
using System.Linq;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Generators;
using FileCabinetApp.Memoization;
using FileCabinetApp.Service;

#pragma warning disable CA1822
namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    /// <summary>
    /// Insert command handler.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private const string InsertConstant = "insert";
        private const string Values = "values";

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public InsertCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, InsertConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Insert(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Insert(string parameters)
        {
            var (properties, values) = this.Parse(parameters);
            var recordPropertyInfo = typeof(FileCabinetRecord).GetProperties();

            var recordGenerator = new RecordGenerator();
            var record = recordGenerator.Generate(1);

            for (int i = 0; i < properties.Length; i++)
            {
                var recordProperty = recordPropertyInfo.FirstOrDefault(prop => string.Equals(prop.Name, properties[i], StringComparison.OrdinalIgnoreCase));
                if (recordProperty is null)
                {
                    continue;
                }

                var converter = TypeDescriptor.GetConverter(recordProperty.PropertyType);
                recordProperty.SetValue(record, converter.ConvertFromInvariantString(values[i]));
            }

            var data = new RecordData()
            {
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                Experience = record.Experience,
                Balance = record.Balance,
                EnglishLevel = record.EnglishLevel,
            };

            this.fileCabinetService.CreateRecord(data);
        }

        private (string[] properties, string[] values) Parse(string parameters)
        {
            var insertArray = parameters.Split(Values);

            if (insertArray.Length != 2)
            {
                throw new ArgumentException($"{InsertConstant} input incorrect.");
            }

            var properties = insertArray[0].Split('(', ')', ',', ' ');
            var values = insertArray[1].Split('(', ')', ',', ' ', '\'');

            values = values.Where(x => x.Length != 0).ToArray();
            properties = properties.Where(x => x.Length != 0).ToArray();

            return (properties, values);
        }
    }
}
