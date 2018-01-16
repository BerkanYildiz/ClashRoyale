namespace ClashRoyale.Handlers.Client.Home
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions.Utils;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Server.Home;

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

            if (Device.GameMode.State <= HomeState.Home)
            {
                var Player = await Players.Get(Device.NetworkManager.AccountId.HigherInt, Device.NetworkManager.AccountId.LowerInt);

                if (Player != null)
                {
                    Device.NetworkManager.SendMessage(new OwnHomeDataMessage(Player, Player.Home, TimeUtil.Timestamp));
                }
                else
                {
                    Logging.Error(typeof(GoHomeHandler), "Player == null at Handle(Device, Message, CancellationToken)..");
                }
            }
            else
            {
                Logging.Error(typeof(GoHomeHandler), "State > HomeState.Home at Handle(Device, Message, CancellationToken)..");
            }
        }
    }
}