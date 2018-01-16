namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Player;

    using Home = ClashRoyale.Logic.Home.Home;

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
        public Home Home;

        public int Timestamp;

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
        /// <param name="Home">The home.</param>
        /// <param name="Timestamp">The timestamp.</param>
        public OwnHomeDataMessage(Player Player, Home Home, int Timestamp)
        {
            this.Player     = Player;
            this.Home       = Home;
            this.Timestamp  = Timestamp;
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
            this.Home.Encode(this.Stream);
            this.Player.Encode(this.Stream);
            
            this.Stream.WriteVInt(-1956785812); // SEED
            this.Stream.WriteVInt(this.Timestamp);
            this.Stream.WriteVInt(626930);
        }
    }
}