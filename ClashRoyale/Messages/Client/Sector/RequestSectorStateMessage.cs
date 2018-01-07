namespace ClashRoyale.Messages.Client.Sector
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class RequestSectorStateMessage : Message
    {
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

        public int ClientTick;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSectorStateMessage"/> class.
        /// </summary>
        public RequestSectorStateMessage()
        {
            // RequestSectorStateMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSectorStateMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public RequestSectorStateMessage(ByteStream Stream) : base(Stream)
        {
            // RequestSectorStateMessage.
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
    }
}