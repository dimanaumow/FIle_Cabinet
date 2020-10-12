using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public void ValidatePararmeters(RecordData parameters);
    }
}
