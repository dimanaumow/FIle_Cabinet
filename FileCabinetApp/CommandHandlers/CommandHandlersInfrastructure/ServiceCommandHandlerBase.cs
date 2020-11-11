using FileCabinetApp.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure
{
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected readonly IFileCabinetService fileCabinetService;

        public ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            if (fileCabinetService is null)
            {
                throw new ArgumentNullException($"{nameof(fileCabinetService)} cannot be null.");
            }

            this.fileCabinetService = fileCabinetService;
        }
    }
}
