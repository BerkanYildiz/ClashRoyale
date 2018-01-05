namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
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

        private readonly Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitedHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Player">The player.</param>
        public VisitedHomeDataMessage(Device Device, Player Player) : base(Device)
        {
            this.Player     = Player;
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

                this.Stream.WriteBoolean(true);
                {
                    this.Stream.WriteVInt(this.Device.GameMode.Home.HighId);
                    this.Stream.WriteVInt(this.Device.GameMode.Home.LowId);
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