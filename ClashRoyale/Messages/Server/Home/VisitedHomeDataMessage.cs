namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Player;

    using Home = ClashRoyale.Logic.Home.Home;

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
        public Home Home;

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
        /// <param name="Home">The home.</param>
        public VisitedHomeDataMessage(Player Player, Home Home)
        {
            this.Player = Player;
            this.Home   = Home;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Stream.ReadVInt();
            this.Stream.ReadVInt();

            if (this.Stream.ReadBoolean())
            {
                this.Home.SpellDeck.Decode(this.Stream);

                this.Player.HighId  = this.Stream.ReadInt();
                this.Player.LowId   = this.Stream.ReadInt();

                if (this.Stream.ReadBoolean())
                {
                    this.Home.HighId = this.Stream.ReadVInt();
                    this.Home.LowId  = this.Stream.ReadVInt();
                }

                this.Stream.ReadVInt();
            }

            if (this.Stream.ReadBoolean())
            {
                // this.Player.Decode();
            }

            if (this.Stream.ReadBoolean())
            {
                // this.Player.Decode();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(1);
            this.Stream.WriteVInt(0);

            this.Stream.WriteBoolean(this.Home != null);

            if (this.Home != null)
            {
                this.Home.SpellDeck.Encode(this.Stream);
                
                this.Stream.WriteLong(this.Player.PlayerId);

                this.Stream.WriteBoolean(true);
                {
                    this.Stream.WriteVInt(this.Player.GameMode.Home.HighId);
                    this.Stream.WriteVInt(this.Player.GameMode.Home.LowId);
                }

                this.Stream.WriteVInt(0);
            }

            this.Stream.WriteBoolean(this.Player != null);

            if (this.Player != null)
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