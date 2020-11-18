using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Exit command handler.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private const string ExitConstant = "exit";
        private Action<bool> isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="isRunning">Delegate isRunning aplication.</param>
        public ExitCommandHandler(Action<bool> isRunning)
        {
            if (isRunning is null)
            {
                throw new ArgumentNullException($"{nameof(isRunning)} cannot be null.");
            }

            this.isRunning = isRunning;
        }

        /// <summary>
        /// Handle command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(ExitConstant, commandRequest.Commands, StringComparison.OrdinalIgnoreCase))
            {
                this.Exit();
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Exit()
        {
            Console.WriteLine("Exiting an application...");
            this.isRunning(false);
        }
    }
}
