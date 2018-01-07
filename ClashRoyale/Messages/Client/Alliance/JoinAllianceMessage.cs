namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class JoinAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14305;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        public int HighId;
        public int LowId;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceMessage"/> class.
        /// </summary>
        public JoinAllianceMessage()
        {
            // JoinAllianceMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public JoinAllianceMessage(ByteStream Stream) : base(Stream)
        {
            // JoinAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.HighId = this.Stream.ReadInt();
            this.LowId  = this.Stream.ReadInt();
            this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.HighId);
            this.Stream.WriteInt(this.LowId);
            this.Stream.WriteInt(0);
        }
    }
}