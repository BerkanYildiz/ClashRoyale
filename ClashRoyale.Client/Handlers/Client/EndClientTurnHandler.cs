namespace ClashRoyale.Handlers.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Home;

    public static class EndClientTurnHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var EndClientTurnMessage = (EndClientTurnMessage) Message;

            if (EndClientTurnMessage == null)
            {
                throw new LogicException(typeof(EndClientTurnHandler), nameof(EndClientTurnMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}