using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>;

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth)
        {
            //TODO: add realization

            return 0;
        }

        public FileCabinetRecord[] GetRecords()
        {
            //TODO: add realization 
            return new FileCabinetRecord[] { };
        }

        public int GetStat()
        {
            //TODO: add realization

            return 0;
        }
    }
}
