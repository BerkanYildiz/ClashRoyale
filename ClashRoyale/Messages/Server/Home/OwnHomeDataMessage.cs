namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Player;

    public class OwnHomeDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 28502;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        public Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        public OwnHomeDataMessage()
        {
            // OwnHomeDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public OwnHomeDataMessage(ByteStream Stream) : base(Stream)
        {
            // OwnHomeDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        public OwnHomeDataMessage(Player Player)
        {
            this.Player = Player;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            // TODO.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Player.Home.Encode(this.Stream);
            this.Player.Encode(this.Stream);

            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
        }
    }
}