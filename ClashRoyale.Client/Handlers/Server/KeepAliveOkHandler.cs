namespace ClashRoyale.Handlers.Server
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Server.Account;

    public static class KeepAliveOkHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var KeepAliveOkMessage = (KeepAliveOkMessage) Message;

            if (KeepAliveOkMessage == null)
            {
                throw new LogicException(typeof(KeepAliveOkHandler), nameof(KeepAliveOkMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.NetworkManager.KeepAliveMessageReceived();
        }
    }
}
