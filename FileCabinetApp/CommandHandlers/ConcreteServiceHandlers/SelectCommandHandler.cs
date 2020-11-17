using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure;
using FileCabinetApp.Comparers;
using FileCabinetApp.Printers;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ConcreteServiceHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        public const string SelectConst = "select";
        private readonly Action<IEnumerable<FileCabinetRecord>, List<string>> printer;

        private bool and;
        private bool or;

        public SelectCommandHandler(IFileCabinetService fileCabinetService)
            : this(fileCabinetService, new TablePrinter().Print)
        {
        }

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
        }

        private void Select(string parameters)
        {
            var (prop, whereProp) = this.Parse(parameters);

            if (whereProp.Count == 0)
            {
                this.printer(this.fileCabinetService.GetRecords(), prop);
                return;
            }

            var finded = new List<List<FileCabinetRecord>>();
            foreach (var item in whereProp)
            {
                if (string.Equals(item.whereProp, "firstName", StringComparison.OrdinalIgnoreCase))
                {
                    var firstNames = new List<FileCabinetRecord>(this.fileCabinetService.FindByFirstName(item.whereVal));
                    finded.Add(firstNames);
                }
                else if (string.Equals(item.whereProp, "lastName", StringComparison.OrdinalIgnoreCase))
                {
                    var lastNames = new List<FileCabinetRecord>(this.fileCabinetService.FindByLastName(item.whereVal));
                    finded.Add(lastNames);
                }
                else if (string.Equals(item.whereProp, "dateOfBirth", StringComparison.OrdinalIgnoreCase))
                {
                    var dates = new List<FileCabinetRecord>(this.fileCabinetService.FindByDateOfBirth(item.whereVal));
                    finded.Add(dates);
                }
                else if (string.Equals(item.whereProp, "experience", StringComparison.OrdinalIgnoreCase))
                {
                    var experiences = new List<FileCabinetRecord>(this.fileCabinetService.FindByExpirience(item.whereVal));
                    finded.Add(experiences);
                }
                else if (string.Equals(item.whereProp, "balance", StringComparison.OrdinalIgnoreCase))
                {
                    var balances = new List<FileCabinetRecord>(this.fileCabinetService.FindByBalance(item.whereVal));
                    finded.Add(balances);
                }
                else if (string.Equals(item.whereProp, "englishLevel", StringComparison.OrdinalIgnoreCase))
                {
                    var levels = new List<FileCabinetRecord>(this.fileCabinetService.FindByEnglishLevel(item.whereVal));
                    finded.Add(levels);
                }
            }

            var selected = finded[0];

            if (this.and)
            {
                foreach (var item in finded)
                {
                    selected = this.Insert(item, selected);
                }
            }
            else if (this.or)
            {
                foreach (var item in finded)
                {
                    selected = this.Union(item, selected);
                }
            }

            this.printer(selected, prop);
        }

        private (List<string>, List<(string whereProp, string whereVal)>) Parse(string parameters)
        {
            var arguments = parameters.Split("where");

            var properies = arguments[0].Split(',').Select(x => x.Trim()).ToList();

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

        private List<FileCabinetRecord> Insert(List<FileCabinetRecord> lhs, List<FileCabinetRecord> rhs)
        {
            var result = new List<FileCabinetRecord>();
            foreach (var lhsItem in lhs)
            {
                foreach (var rhsItem in rhs)
                {
                    if (lhsItem.FirstName == rhsItem.FirstName &&
                        lhsItem.LastName == rhsItem.LastName &&
                        lhsItem.DateOfBirth == rhsItem.DateOfBirth &&
                        lhsItem.Experience == rhsItem.Experience &&
                        lhsItem.Balance == rhsItem.Balance &&
                        lhsItem.EnglishLevel == rhsItem.EnglishLevel)
                    {
                        result.Add(lhsItem);
                    }
                }
            }

            return result;
        }

        private List<FileCabinetRecord> Union(List<FileCabinetRecord> lhs, List<FileCabinetRecord> rhs)
        {
            var result = new List<FileCabinetRecord>(lhs.Count);

            var lhsSet = new SortedSet<FileCabinetRecord>(lhs, new RecordComparer());
            var rhsSet = new SortedSet<FileCabinetRecord>(rhs, new RecordComparer());

            lhsSet.UnionWith(rhsSet);

            foreach (var item in lhsSet)
            {
                result.Add(item);
            }

            return result;
        }
    }
}
