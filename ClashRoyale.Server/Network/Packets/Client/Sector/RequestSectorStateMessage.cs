namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class RequestSectorStateMessage : Message
    {
        internal int ClientTick;

        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 12903;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        internal override void Decode()
        {
            this.ClientTick = this.Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.ClientTick);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (this.Device.GameMode.State == HomeState.Attack)
            {
                Logging.Info(this.GetType(), "Client ask SectorStateMessage. Avatar id: " + this.Device.NetworkManager.AccountId);
            }
        }
    }
}