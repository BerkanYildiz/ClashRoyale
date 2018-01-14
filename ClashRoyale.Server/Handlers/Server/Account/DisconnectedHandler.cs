namespace ClashRoyale.Handlers.Server.Account
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Server.Account;

    public static class DisconnectedHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var DisconnectedMessage = (DisconnectedMessage) Message;

            if (DisconnectedMessage == null)
            {
                throw new LogicException(typeof(DisconnectedHandler), nameof(DisconnectedMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.State = State.Disconnected;
        }
    }
}
