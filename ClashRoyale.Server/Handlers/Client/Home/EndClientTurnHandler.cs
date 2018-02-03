namespace ClashRoyale.Handlers.Client.Home
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Player;
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

            if (EndClientTurnMessage.Commands != null)
            {
                EndClientTurnMessage.Commands.ForEach(Command =>
                {
                    if (Command.ExecuteTick <= EndClientTurnMessage.Tick)
                    {
                        Device.GameMode.CommandManager.AddCommand(Command);
                    }
                });
            }

            for (int I = Device.GameMode.Time; I < EndClientTurnMessage.Tick; I++)
            {
                Device.GameMode.UpdateOneTick();
            }

            Player Player = Device.GameMode.Player;

            if (Player.IsInAlliance)
            {
                Clan Clan = await Clans.Get(Player.ClanHighId, Player.ClanLowId);

                if (Clan != null)
                {
                    if (Clan.Members.TryGetValue(Player.PlayerId, out var Member))
                    {
                        Member.SetPlayer(Player);
                    }
                }
                else
                {
                    Logging.Error(typeof(EndClientTurnHandler), "Clan == null at Handle(Device, Message, CancellationToken).");
                }
            }

            /* if (Device.GameMode.State == HomeState.Home)
            {
                if (EndClientTurnMessage.Checksum != Device.GameMode.Checksum)
                {
                    Logging.Error(this.GetType(), "Player is out of sync (S: " + Device.GameMode.Checksum + ", C: " + EndClientTurnMessage.Checksum + ").");
                    Device.NetworkManager.SendMessage(new OutOfSyncMessage(Device, EndClientTurnMessage.Checksum, Device.GameMode.Checksum));
                }
            } */
        }
    }
}