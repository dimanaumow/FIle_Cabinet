using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class NationalityValidator : IRecordValidator
    {
        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            if (!char.IsLetter(parameters.nationality))
            {
                throw new ArgumentException($"{nameof(parameters.nationality)} must be letter.");
            }
        }
    }
}
