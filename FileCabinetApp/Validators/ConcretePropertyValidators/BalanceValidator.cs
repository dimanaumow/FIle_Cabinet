using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class BalanceValidator : IRecordValidator
    {
        private readonly decimal min;

        public BalanceValidator(decimal min)
        {
            this.min = min;
        }

        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (parameters.balance < this.min)
            {
                throw new ArgumentException($"{nameof(parameters.balance)} must be greatest than {this.min}");
            }
        }
    }
}
