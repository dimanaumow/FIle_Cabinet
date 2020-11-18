using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The balance validator.
    /// </summary>
    public class BalanceValidator : IRecordValidator
    {
        private readonly decimal min;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceValidator"/> class.
        /// </summary>
        /// <param name="min">The min balance.</param>
        public BalanceValidator(decimal min)
        {
            this.min = min;
        }

        /// <summary>
        /// Valadate parameters.
        /// </summary>
        /// <param name="parameters">Record data.</param>
        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (parameters.Balance < this.min)
            {
                throw new ArgumentException($"{nameof(parameters.Balance)} must be greatest than {this.min}");
            }
        }
    }
}
