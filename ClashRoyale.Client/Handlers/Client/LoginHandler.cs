namespace ClashRoyale.Handlers.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;

    public static class LoginHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var LoginMessage = (LoginMessage) Message;

            if (LoginMessage == null)
            {
                throw new LogicException(typeof(LoginHandler), nameof(LoginMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.State = State.Login;
        }
    }
}
