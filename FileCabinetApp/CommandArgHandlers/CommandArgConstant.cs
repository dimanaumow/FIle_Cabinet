#pragma warning disable
namespace FileCabinetApp.CommandArgHandlers
{
    /// <summary>
    /// Provide constant for work with command arguments.
    /// </summary>
    public static class CommandArgConstant
    {
        /// <summary>
        /// Validation rule.
        /// </summary>
        public const string VALIDATIONRULE = "--validation-rules";

        /// <summary>
        /// Validation rule in short form.
        /// </summary>
        public const string VALIDATIONRULESHORT = "-v";

        /// <summary>
        /// Storage.
        /// </summary>
        public const string STORAGE = "--storage";

        /// <summary>
        /// Storage in short form.
        /// </summary>
        public const string STORAGESHORT = "-s";

        /// <summary>
        /// Stopwatch.
        /// </summary>
        public const string STOPWATCH = "-use-stopwatch";

        /// <summary>
        /// Stopwatch in short form.
        /// </summary>
        public const string LOGGER = "-use-logger";

        /// <summary>
        /// Single commands.
        /// </summary>
        public static string[] SingleComand = { STOPWATCH, LOGGER };
    }
}
