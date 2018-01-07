namespace ClashRoyale.Handlers.Server
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Server.Account;

    public static class ServerShutdownHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var ServerShutdownMessage = (ServerShutdownMessage) Message;

            if (ServerShutdownMessage == null)
            {
                throw new LogicException(typeof(ServerShutdownHandler), nameof(ServerShutdownMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}
