namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class OutOfSyncMessage : Message
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

        public int ClientChecksum;
        public int ServerChecksum;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfSyncMessage"/> class.
        /// </summary>
        public OutOfSyncMessage()
        {
            // OutOfSyncMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfSyncMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public OutOfSyncMessage(ByteStream Stream) : base(Stream)
        {
            // OutOfSyncMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfSyncMessage"/> class.
        /// </summary>
        /// <param name="ClientChecksum">The client checksum.</param>
        /// <param name="ServerChecksum">The server checksum.</param>
        public OutOfSyncMessage(int ClientChecksum, int ServerChecksum)
        {
            this.ClientChecksum = ClientChecksum;
            this.ServerChecksum = ServerChecksum;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ServerChecksum = this.Stream.ReadVInt();
            this.ClientChecksum = this.Stream.ReadVInt();
            this.Stream.ReadVInt();
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
    }
}