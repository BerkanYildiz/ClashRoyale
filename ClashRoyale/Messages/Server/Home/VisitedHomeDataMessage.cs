namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Player;

    public class VisitedHomeDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 25880;
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
        /// Initializes a new instance of the <see cref="VisitedHomeDataMessage"/> class.
        /// </summary>
        public VisitedHomeDataMessage()
        {
            // VisitedHomeDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitedHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public VisitedHomeDataMessage(ByteStream Stream) : base(Stream)
        {
            // VisitedHomeDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitedHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        public VisitedHomeDataMessage(Player Player)
        {
            this.Player = Player;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(1);
            this.Stream.WriteVInt(0);

            this.Stream.WriteBoolean(true);
            {
                this.Player.Home.SpellDeck.Encode(this.Stream);
                
                this.Stream.WriteLong(this.Player.PlayerId);

                this.Stream.WriteBoolean(false);
                {
                    this.Stream.WriteVInt(this.Player.GameMode.Home.HighId);
                    this.Stream.WriteVInt(this.Player.GameMode.Home.LowId);
                }

                this.Stream.WriteVInt(0);
            }

            this.Stream.WriteBoolean(true);
            {
                this.Player.Encode(this.Stream);
            }

            this.Stream.WriteBoolean(false);
            {
                // this.Player.Encode(this.Stream);
            }
        }
    }
}