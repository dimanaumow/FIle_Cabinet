using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Provide interface for different validation rule.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validation rule for record parameters.
        /// </summary>
        /// <param name="parameters">User's data.</param>
        public void ValidatePararmeters(RecordData parameters);
    }
}
