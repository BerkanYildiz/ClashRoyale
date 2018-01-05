namespace ClashRoyale.Server.Handlers.Server
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Server.Account;

    public static class ServerHelloHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var ServerHello = (ServerHelloMessage) Message;

            if (ServerHello == null)
            {
                throw new LogicException(typeof(ServerHelloHandler), "ServerHello == null at Handle(Device, Message, CancellationToken).");
            }

            if (ServerHello.SessionKey == null)
            {
                throw new LogicException(typeof(ServerHelloHandler), "ServerHello.SessionKey == null at Handle(Device, Message, CancellationToken).");
            }

            if (Device.NetworkManager.PepperInit.SessionKey != null)
            {
                Logging.Warning(typeof(ServerHelloHandler), "Device.SessionKey != null at Handle(Device, Message, CancellationToken).");
            }

            Device.NetworkManager.PepperInit.SessionKey = ServerHello.SessionKey;
        }
    }
}