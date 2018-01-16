namespace ClashRoyale.Handlers.Client.RoyalTv
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.RoyalTv;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.RoyalTv;
    using ClashRoyale.Messages.Server.RoyalTv;

    public class AskForTvContentHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var AskForTvContentMessage = (AskForTvContentMessage) Message;

            if (AskForTvContentMessage == null)
            {
                throw new LogicException(typeof(AskForTvContentHandler), nameof(AskForTvContentMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            int ChannelId = RoyalTvManager.GetChannelArenaData(AskForTvContentMessage.ArenaData);

            if (ChannelId == -1)
            {
                throw new LogicException(typeof(AskForTvContentHandler), "ChannelId == -1 at Handle(Device, Message, CancellationToken).");
            }

            var Channel = RoyalTvManager.Channels[ChannelId];

            if (Channel == null)
            {
                throw new LogicException(typeof(AskForTvContentHandler), "Channel == null at Handle(Device, Message, CancellationToken).");
            }

            var RoyalTv = Channel.ToArray();

            Device.NetworkManager.SendMessage(new RoyalTvContentMessage(RoyalTv, AskForTvContentMessage.ArenaData));
        }
    }
}
