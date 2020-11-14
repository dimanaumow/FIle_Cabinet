using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandArgHandlers
{
    public static class CommandArgConstant
    {
        public const string VALIDATIONRULE = "--validation-rules";
        public const string VALIDATIONRULESHORT = "-v";
        public const string STORAGE = "--storage";
        public const string STORAGESHORT = "-s";
        public const string STOPWATCH = "-use-stopwatch";
        public const string LOGGER = "-use-logger";

        public static string[] singleComand = { STOPWATCH, LOGGER};
    }
}
