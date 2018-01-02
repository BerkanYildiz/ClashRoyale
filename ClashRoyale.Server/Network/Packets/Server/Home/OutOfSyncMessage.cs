namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class OutOfSyncMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24104;
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

        private readonly int ClientChecksum;
        private readonly int ServerChecksum;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfSyncMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ClientChecksum">The client checksum.</param>
        /// <param name="ServerChecksum">The server checksum.</param>
        public OutOfSyncMessage(Device Device, int ClientChecksum, int ServerChecksum) : base(Device)
        {
            this.ClientChecksum = ClientChecksum;
            this.ServerChecksum = ServerChecksum;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.ServerChecksum);
            this.Stream.WriteVInt(this.ClientChecksum);
            this.Stream.WriteVInt(0);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            this.Device.State = State.Disconnected;
        }
    }
}