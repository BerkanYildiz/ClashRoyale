namespace ClashRoyale.Handlers.Client.Avatar
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Server.Home;

    public static class AskForBattleReplayStreamHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var AskForBattleReplayStreamMessage = (AskForBattleReplayStreamMessage) Message;

            if (AskForBattleReplayStreamMessage == null)
            {
                throw new LogicException(typeof(AskForBattleReplayStreamHandler), nameof(AskForBattleReplayStreamMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            var Player = (Player) null;

            if (AskForBattleReplayStreamMessage.PlayerId.IsZero)
            {
                Player = Device.GameMode.Player;
            }
            else
            {
                Player = await Players.Get(AskForBattleReplayStreamMessage.PlayerId.HigherInt, AskForBattleReplayStreamMessage.PlayerId.LowerInt, false);

                if (Player == null)
                {
                    Player = Device.GameMode.Player;
                }
            }

            // TODO : Implement the BattleReport().

            Device.NetworkManager.SendMessage(new BattleReportStreamMessage()
            {
                PlayerId = Player.PlayerId
            });
        }
    }
}