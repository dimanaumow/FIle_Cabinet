using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Incapsuleted the command request.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">The command name.</param>
        /// <param name="parameters">The command parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            if (command is null)
            {
                throw new ArgumentNullException($"{nameof(command)} cannot be null.");
            }

            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            this.Commands = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets commands.
        /// </summary>
        /// <value>the command name.</value>
        public string Commands { get; }

        /// <summary>
        /// Gets command parameters.
        /// </summary>
        /// <value>The command parameters.</value>
        public string Parameters { get; }
    }
}