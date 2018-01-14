namespace ClashRoyale.Handlers.Client.Matchmaking
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Matchmaking;
    using ClashRoyale.Messages.Server.Matchmaking;

    public static class CancelMatchmakeHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var CancelMatchmakeMessage = (CancelMatchmakeMessage) Message;

            if (CancelMatchmakeMessage == null)
            {
                throw new LogicException(typeof(CancelMatchmakeHandler), nameof(CancelMatchmakeMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (BattleManager.Waitings.TryRemove(Device.GameMode.Player.PlayerId, out _))
            {
                Device.NetworkManager.SendMessage(new CancelMatchmakeDoneMessage());
            }
        }
    }
}
