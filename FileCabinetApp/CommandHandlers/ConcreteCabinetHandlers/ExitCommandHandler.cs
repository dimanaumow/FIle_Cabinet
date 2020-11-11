using System;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        public const string ExitConstant = "exit";
        private Action<bool> isRunning;

        public ExitCommandHandler(Action<bool> isRunning)
        {
            if (isRunning is null)
            {
                throw new ArgumentNullException($"{nameof(isRunning)} cannot be null.");
            }

            this.isRunning = isRunning;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(ExitConstant, commandRequest.Commands, StringComparison.OrdinalIgnoreCase))
            {
                this.Exit(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            this.isRunning(false);
        }
    }
}
