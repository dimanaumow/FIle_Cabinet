using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Composite validator.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">The validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            ICollection<IRecordValidator> collection = validators as ICollection<IRecordValidator>;

            if (collection is null)
            {
                throw new ArgumentException($"{nameof(validators)} must be collection.");
            }

            this.validators = new List<IRecordValidator>(collection);
        }

        /// <summary>
        /// Validate data parameters.
        /// </summary>
        /// <param name="parameters">Record data.</param>
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
