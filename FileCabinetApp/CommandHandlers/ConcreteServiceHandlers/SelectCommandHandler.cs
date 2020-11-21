using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Comparers;
using FileCabinetApp.Printers;
using FileCabinetApp.Service;

#pragma warning disable CA1822
namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    /// <summary>
    /// Select command handler.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private const string SelectConst = "select";
        private readonly Action<IEnumerable<FileCabinetRecord>, List<string>> printer;

        private bool and;
        private bool or;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService)
            : this(fileCabinetService, new TablePrinter().Print)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The cureent service.</param>
        /// <param name="printer">The current printer.</param>
        public SelectCommandHandler(
            IFileCabinetService fileCabinetService,
            Action<IEnumerable<FileCabinetRecord>, List<string>> printer)
            : base(fileCabinetService)
        {
            if (printer is null)
            {
                throw new ArgumentNullException($"{nameof(printer)} cannot be null.");
            }

            this.printer = printer;
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

            if (string.Equals(commandRequest.Commands, SelectConst, StringComparison.OrdinalIgnoreCase))
            {
                this.Select(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Select(string parameters)
        {
            var (prop, whereProp) = this.Parse(parameters);

            if (whereProp.Count == 0)
            {
                this.printer(this.fileCabinetService.GetRecords(), prop);
                return;
            }

            var condition = new WhereConditions()
            {
                FirstName = null,
                LastName = null,
                DateOfBirth = null,
                Experience = null,
                Balance = null,
                EnglishLevel = null,
            };

            this.CreateCondition(whereProp, ref condition);

            if (this.or)
            {
                var selected = this.fileCabinetService.FindByOr(condition);
                this.printer(selected, prop);
            }
            else if (this.and)
            {
                var selected = this.fileCabinetService.FindByAnd(condition);
                this.printer(selected, prop);
            }
        }

        private (List<string>, List<(string whereProp, string whereVal)>) Parse(string parameters)
        {
            var arguments = parameters.Split("where");

            var properies = arguments[0].Split(',').Select(x => x.Trim()).ToList();

            if (arguments.Length == 1)
            {
                return (properies, new List<(string whereProp, string whereVal)>());
            }

            var whereProperties = this.WhereParse(arguments[1]);

            return (properies, whereProperties);
        }

        private List<(string whereProp, string whereVal)> WhereParse(string parameters)
        {
            var result = new List<(string prop, string val)>();

            if (parameters.Contains(" and ", StringComparison.Ordinal))
            {
                this.and = true;
                var arguments = parameters.Split("and");

                foreach (var arg in arguments)
                {
                    var whereValues = arg.Split("=");
                    result.Add((whereValues[0].Trim(), whereValues[1].Trim('\'', ' ')));
                }
            }
            else if (parameters.Contains(" or ", StringComparison.Ordinal))
            {
                this.or = true;
                var arguments = parameters.Split("or");

                foreach (var arg in arguments)
                {
                    var whereValues = arg.Split("=");
                    result.Add((whereValues[0].Trim(), whereValues[1].Trim('\'', ' ')));
                }
            }
            else
            {
                var whereValues = parameters.Split("=");
                result.Add((whereValues[0].Trim(), whereValues[1].Trim('\'', ' ')));
            }

            return result;
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
    }
}
