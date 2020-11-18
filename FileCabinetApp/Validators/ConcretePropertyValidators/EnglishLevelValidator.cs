using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// English level validator.
    /// </summary>
    public class EnglishLevelValidator : IRecordValidator
    {
        private readonly string levels;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnglishLevelValidator"/> class.
        /// </summary>
        /// <param name="levels">The levels.</param>
        public EnglishLevelValidator(string levels)
        {
            if (levels is null)
            {
                throw new ArgumentNullException($"{nameof(levels)} cannot be null.");
            }

            this.levels = levels;
        }

        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="parameters">The record data.</param>
        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (!this.levels.Contains(parameters.englishLevel, StringComparison.Ordinal))
            {
                throw new ArgumentException($"{nameof(parameters.englishLevel)} must be correct english level.");
            }
        }
    }
}
