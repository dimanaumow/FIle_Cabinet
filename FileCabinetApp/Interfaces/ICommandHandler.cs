namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Set next command handler.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <returns>The current command handler.</returns>
        ICommandHandler SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        void Handle(AppCommandRequest commandRequest);
    }
}
