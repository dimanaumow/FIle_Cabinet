using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.Memoization
{
    public static class CashedData 
    {
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> dateOfBirtCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> firstNameCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> lastNameCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> experienceCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> balanceCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> englishLevelCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        public static void ClearCashe()
        {
            dateOfBirtCashe.Clear();
            firstNameCashe.Clear();
            lastNameCashe.Clear();
            experienceCashe.Clear();
            balanceCashe.Clear();
            englishLevelCashe.Clear();
        }
    }
}
