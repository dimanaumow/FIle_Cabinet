using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler commandHandler;

        public virtual void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (!(this.commandHandler is null))
            {
                this.commandHandler.Handle(commandRequest);
            }
            else
            {
                Console.WriteLine($"There is no '{commandRequest.Commands}' command.");
                CommandPromt.CommandPromtHandler.GetTheMostSimular(commandRequest.Commands);
                Console.WriteLine();
            }
        }

        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
            return this.commandHandler;
        }
    }
}
