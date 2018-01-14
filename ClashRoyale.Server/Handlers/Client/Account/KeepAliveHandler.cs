namespace ClashRoyale.Handlers.Client.Account
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;
    using ClashRoyale.Messages.Server.Account;

    public static class KeepAliveHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var KeepAliveMessage = (KeepAliveMessage) Message;

            if (KeepAliveMessage == null)
            {
                throw new LogicException(typeof(KeepAliveHandler), nameof(KeepAliveMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.NetworkManager.KeepAliveMessageReceived();
            Device.NetworkManager.SendMessage(new KeepAliveOkMessage());
        }
    }
}
