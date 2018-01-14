namespace ClashRoyale.Handlers.Client.Alliance
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Alliance.Entries;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Alliance;
    using ClashRoyale.Messages.Server.Alliance;

    public static class AskForAllianceDataHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var AskForAllianceDataMessage = (AskForAllianceDataMessage) Message;

            if (AskForAllianceDataMessage == null)
            {
                throw new LogicException(typeof(AskForAllianceDataHandler), nameof(AskForAllianceDataMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Clan Clan = await Clans.Get(AskForAllianceDataMessage.HighId, AskForAllianceDataMessage.LowId);

            if (Clan != null)
            {
                Device.NetworkManager.SendMessage(new AllianceDataMessage(new AllianceFullEntry()
                {
                    Header      = Clan.HeaderEntry,
                    Members     = Clan.Members.Values.ToArray(),
                    Description = Clan.Description
                }));
            }
            else
            {
                Logging.Warning(typeof(AskForAllianceDataHandler), "Clan == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}
