namespace ClashRoyale.Handlers.Server.Account
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
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
            var ServerHelloMessage = (ServerHelloMessage) Message;

            if (ServerHelloMessage == null)
            {
                throw new LogicException(typeof(ServerHelloHandler), nameof(ServerHelloMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.State = State.SessionOk;

            if (ServerHelloMessage.SessionKey == null)
            {
                throw new LogicException(typeof(ServerHelloHandler), "ServerHelloMessage.SessionKey == null at Handle(Device, Message, CancellationToken).");
            }

            if (Device.NetworkManager.PepperInit.SessionKey != null)
            {
                Logging.Warning(typeof(ServerHelloHandler), "Device.SessionKey != null at Handle(Device, Message, CancellationToken).");
            }

            Device.NetworkManager.PepperInit.SessionKey = ServerHelloMessage.SessionKey;
        }
    }
}