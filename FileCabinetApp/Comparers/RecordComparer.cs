using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.Comparers
{
    public class RecordComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord lhs, FileCabinetRecord rhs)
            => lhs.Id - rhs.Id;
    }
}
