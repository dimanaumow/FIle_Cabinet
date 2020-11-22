using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Memoization;
using FileCabinetApp.Service;

#pragma warning disable CA1822
namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    /// <summary>
    /// Update handler.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private const string UpdateConstant = "update";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(commandRequest.Commands, UpdateConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Update(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Update(string parameters)
        {
            var (newProp, whereProp) = this.Parse(parameters);

            var set = newProp;
            var where = whereProp;

            var condition = new WhereConditions()
            {
                FirstName = null,
                LastName = null,
                DateOfBirth = null,
                Experience = null,
                Balance = null,
                EnglishLevel = null,
            };

            this.CreateCondition(where, ref condition);
            var updated = this.fileCabinetService.FindByAnd(condition);

            foreach (var record in updated)
            {
                RecordData data = this.CreateDataForEditing(record, set);
                this.fileCabinetService.EditRecord(record.Id, data);
            }

            CashedData.ClearCashe();
        }

        private (List<(string prop, string val)>, List<(string whereProp, string whereVal)>) Parse(string parameters)
        {
            var listNew = new List<(string, string)>();
            parameters = parameters.Substring(3);

            var arguments = parameters.Split("where");

            var newPropValue = arguments[0].Split(',');

            foreach (var item in newPropValue)
            {
                var itemArg = item.Split("=");
                listNew.Add((itemArg[0].Trim(' ', '\''), itemArg[1].Trim('\'', ' ')));
            }

            var listWhere = this.WhereParse(arguments[1]);

            return (listNew, listWhere);
        }

        private List<(string whereProp, string whereVal)> WhereParse(string parameters)
        {
            var arguments = parameters.Split("and");

            var listWhere = new List<(string, string)>();

            foreach (var arg in arguments)
            {
                var whereValues = arg.Split("=");
                listWhere.Add((whereValues[0].Trim(' ', '\''), whereValues[1].Trim('\'', ' ')));
            }

            return listWhere;
        }

        private void CreateCondition(List<(string whereProp, string whereVal)> createParameters, ref WhereConditions conditions)
        {
            foreach (var item in createParameters)
            {
                if (string.Equals(item.whereProp, "firstName", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.FirstName = item.whereVal;
                }
                else if (string.Equals(item.whereProp, "lastName", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.LastName = item.whereVal;
                }
                else if (string.Equals(item.whereProp, "dateOfBirth", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.DateOfBirth = DateTime.Parse(item.whereVal, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.whereProp, "experience", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.Experience = short.Parse(item.whereVal, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.whereProp, "balance", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.Balance = decimal.Parse(item.whereVal, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.whereProp, "englishLevel", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.EnglishLevel = item.whereVal[0];
                }
            }
        }

        private RecordData CreateDataForEditing(FileCabinetRecord record, List<(string prop, string value)> editParameters)
        {
            RecordData data = new RecordData()
            {
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                Experience = record.Experience,
                EnglishLevel = record.EnglishLevel,
                Balance = record.Balance,
            };

            foreach (var item in editParameters)
            {
                if (string.Equals(item.prop, "firstName", StringComparison.OrdinalIgnoreCase))
                {
                    data.FirstName = item.value;
                }
                else if (string.Equals(item.prop, "lastName", StringComparison.OrdinalIgnoreCase))
                {
                    data.LastName = item.value;
                }
                else if (string.Equals(item.prop, "dateofbirth", StringComparison.OrdinalIgnoreCase))
                {
                    data.DateOfBirth = DateTime.Parse(item.value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.prop, "experience", StringComparison.OrdinalIgnoreCase))
                {
                    data.Experience = short.Parse(item.value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.prop, "balance", StringComparison.OrdinalIgnoreCase))
                {
                    data.Balance = decimal.Parse(item.value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.prop, "englishlevel", StringComparison.OrdinalIgnoreCase))
                {
                    data.EnglishLevel = item.value[0];
                }
            }

            return data;
        }
    }
}
