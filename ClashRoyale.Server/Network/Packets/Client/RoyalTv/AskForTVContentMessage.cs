namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Linq;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.RoyalTV;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForTvContentMessage : Message
    {
        internal ArenaData ArenaData;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14402;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.RoyalTv;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForTvContentMessage"/> class.
        /// </summary>
        public AskForTvContentMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForTVContentMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.ArenaData = this.Stream.DecodeData<ArenaData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.EncodeData(this.ArenaData);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (this.ArenaData != null)
            {
                int ChannelIdx = RoyalTvManager.GetChannelArenaData(this.ArenaData);

                if (ChannelIdx != -1)
                {
                    this.Device.NetworkManager.SendMessage(new RoyalTvContentMessage(this.Device, RoyalTvManager.Channels[ChannelIdx].ToList(), this.ArenaData));
                }
                else
                {
                    Logging.Info(this.GetType(), "Channel doesn't exist. (arena: " + this.ArenaData.Name + ")");
                }
            }
            else
            {
                Logging.Info(this.GetType(), "Arena data is NULL.");
            }
        }
    }
}