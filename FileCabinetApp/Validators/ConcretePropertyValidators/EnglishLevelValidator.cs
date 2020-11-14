using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class EnglishLevelValidator : IRecordValidator
    {
        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (!(parameters.englishLevel == 'a' || parameters.englishLevel == 'b' || parameters.englishLevel == 'c'))
            {
                throw new ArgumentException($"{nameof(parameters.englishLevel)} must be letter.");
            }
        }
    }
}
