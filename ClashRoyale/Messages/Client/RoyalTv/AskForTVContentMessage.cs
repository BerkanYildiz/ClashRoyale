namespace ClashRoyale.Messages.Client.RoyalTv
{
    using System.Linq;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.RoyalTV;
    using ClashRoyale.Messages.Server.RoyalTv;

    public class AskForTvContentMessage : Message
    {
        internal ArenaData ArenaData;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10185;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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
        public override void Decode()
        {
            this.ArenaData = this.Stream.DecodeData<ArenaData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.EncodeData(this.ArenaData);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
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