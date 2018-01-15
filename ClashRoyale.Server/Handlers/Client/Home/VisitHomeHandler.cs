namespace ClashRoyale.Handlers.Client.Home
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Server.Home;

    public static class VisitHomeHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var VisitHomeMessage = (VisitHomeMessage) Message;

            if (VisitHomeMessage == null)
            {
                throw new LogicException(typeof(VisitHomeHandler), nameof(VisitHomeMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Player Player = await Players.Get(VisitHomeMessage.HighId, VisitHomeMessage.LowId, false);

            if (Player != null)
            {
                Device.NetworkManager.SendMessage(new VisitedHomeDataMessage(Player, Player.Home));
            }
            else
            {
                Logging.Error(typeof(VisitHomeHandler), "Player(" + VisitHomeMessage.HighId + "-" + VisitHomeMessage.LowId + ") == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}