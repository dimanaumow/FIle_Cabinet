using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class EnglishLevelValidator : IRecordValidator
    {
        private readonly string levels;

        public EnglishLevelValidator(string levels)
        {
            if (levels is null)
            {
                throw new ArgumentNullException($"{nameof(levels)} cannot be null.");
            }

            this.levels = levels;
        }


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
