namespace ClashRoyale.Handlers.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Home;

    public static class GoHomeHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var GoHomeMessage = (GoHomeMessage) Message;

            if (GoHomeMessage == null)
            {
                throw new LogicException(typeof(GoHomeHandler), nameof(GoHomeMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.GameMode.State = HomeState.Home;
        }
    }
}