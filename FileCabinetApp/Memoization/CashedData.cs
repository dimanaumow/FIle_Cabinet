using System.Collections.Generic;
using FileCabinetApp.Service;

namespace FileCabinetApp.Memoization
{
    /// <summary>
    /// Cache data calculation.
    /// </summary>
    public static class CashedData
    {
        /// <summary>
        /// Date cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> DateOfBirtCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// FirstName cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> FirstNameCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// LastName cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> LastNameCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Experience cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> ExperienceCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Balance cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> BalanceCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// English level cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> EnglishLevelCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Clear all cache.
        /// </summary>
        public static void ClearCashe()
        {
            DateOfBirtCashe.Clear();
            FirstNameCashe.Clear();
            LastNameCashe.Clear();
            ExperienceCashe.Clear();
            BalanceCashe.Clear();
            EnglishLevelCashe.Clear();
        }
    }
}
