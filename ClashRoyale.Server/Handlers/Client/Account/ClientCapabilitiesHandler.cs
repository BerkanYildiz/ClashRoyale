namespace ClashRoyale.Handlers.Client.Account
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;

    public static class ClientCapabilitiesHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var ClientCapabilitiesMessage = (ClientCapabilitiesMessage) Message;

            if (ClientCapabilitiesMessage == null)
            {
                throw new LogicException(typeof(ClientCapabilitiesHandler), nameof(ClientCapabilitiesMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (ClientCapabilitiesMessage.Ping <= 0)
            {
                Logging.Info(typeof(ClientCapabilitiesHandler), "Ping <= 0 at Handle(Device, Message, CancellationToken).");
            }

            if (string.IsNullOrEmpty(ClientCapabilitiesMessage.Interface))
            {
                Logging.Info(typeof(ClientCapabilitiesHandler), "Interface == null or empty at Handle(Device, Message, CancellationToken).");
            }

            Device.NetworkManager.Ping      = ClientCapabilitiesMessage.Ping;
            Device.NetworkManager.Interface = ClientCapabilitiesMessage.Interface;
        }
    }
}
