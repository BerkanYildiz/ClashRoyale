namespace ClashRoyale.Handlers.Server.Home
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Server.Home;

    public static class OwnHomeDataHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var OwnHomeDataMessage = (OwnHomeDataMessage) Message;

            if (OwnHomeDataMessage == null)
            {
                throw new LogicException(typeof(OwnHomeDataHandler), nameof(OwnHomeDataMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            var Player  = OwnHomeDataMessage.Player;
            var Home    = OwnHomeDataMessage.Home;

            if (Player != null && Home != null)
            {
                Device.GameMode.LoadHomeState(Player, Home.SecondsSinceLastSave, 113);
                Device.GameMode.Home.LastTick = DateTime.UtcNow;
            }
            else
            {
                Logging.Info(typeof(OwnHomeDataHandler), "Player OR Home == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}
