using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        ICommandHandler SetNext(ICommandHandler commandHandler);

        void Handle(AppCommandRequest commandRequest);
    }
}
