namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class RequestSectorStateMessage : Message
    {
        internal int ClientTick;

        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12903;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Sector;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSectorStateMessage"/> class.
        /// </summary>
        public RequestSectorStateMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // RequestSectorStateMessage   
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ClientTick = this.Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.ClientTick);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            if (this.Device.GameMode.State == HomeState.Attack)
            {
                Logging.Info(this.GetType(), "Client ask SectorStateMessage. Avatar id: " + this.Device.NetworkManager.AccountId);
            }
        }
    }
}