namespace ClashRoyale.Handlers.Server.Home
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Server.Home;

    public static class ServerErrorHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var ServerErrorMessage = (ServerErrorMessage) Message;

            if (ServerErrorMessage == null)
            {
                throw new LogicException(typeof(ServerErrorHandler), nameof(ServerErrorMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.State = State.Disconnected;
        }
    }
}
