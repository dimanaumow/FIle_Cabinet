﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            ICollection<IRecordValidator> collection = validators as ICollection<IRecordValidator>;

            if (collection is null)
            {
                throw new ArgumentException($"{nameof(validators)} must be collection.");
            }

            this.validators = new List<IRecordValidator>(collection);
        }

        public void ValidatePararmeters(RecordData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            foreach (var validator in this.validators)
            {
                validator.ValidatePararmeters(parameters);
            }
        }
    }
}