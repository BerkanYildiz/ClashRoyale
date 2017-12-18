namespace ClashRoyale.Client.Core.Network
{
    using ClashRoyale.Client.Packets;

    internal static class Processor
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="Command">The command.</param>
        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}