namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages;

    internal class DeviceAlreadyBoundMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24262;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        private readonly Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceAlreadyBoundMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Player">The player.</param>
        public DeviceAlreadyBoundMessage(Device Device, Player Player) : base(Device)
        {
            this.Player = Player;
        }

        /// <summary>
        /// Encodes the <see cref="Message" />, using the <see cref="Writer" /> instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(null);
            this.Stream.WriteBoolean(true);

            this.Stream.WriteInt(this.Player.HighId);
            this.Stream.WriteInt(this.Player.LowId);

            this.Stream.WriteString(this.Player.Token);

            this.Player.Encode(this.Stream);
        }
    }
}
