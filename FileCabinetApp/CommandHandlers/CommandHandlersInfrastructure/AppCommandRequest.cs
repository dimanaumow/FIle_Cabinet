using System;

namespace FileCabinetApp
{
    public class AppCommandRequest
    {
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

        public string Commands { get; }

        public string Parameters { get; }
    }
}