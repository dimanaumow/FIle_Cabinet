using System;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.CommandHandlersInfrastructure
{
    /// <summary>
    /// Base command hanlder for services command.
    /// </summary>
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// The current service.
        /// </summary>
        protected readonly IFileCabinetService fileCabinetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
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
