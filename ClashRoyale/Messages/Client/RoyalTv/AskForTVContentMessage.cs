namespace ClashRoyale.Messages.Client.RoyalTv
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    public class AskForTvContentMessage : Message
    {
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

        public ArenaData ArenaData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForTvContentMessage"/> class.
        /// </summary>
        public AskForTvContentMessage()
        {
            // AskForTvContentMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForTvContentMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForTvContentMessage(ByteStream Stream) : base(Stream)
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
    }
}