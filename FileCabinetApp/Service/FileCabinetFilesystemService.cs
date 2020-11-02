using System;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        public int CreateRecord(RecordData parameters)
        {
            throw new NotImplementedException();
        }

        public void EditRecord(int id, RecordData parameters)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            throw new NotImplementedException();
        }
    }
}
