namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class ReplyJoinRequestMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14321;
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

        /// <summary>
        /// Gets the stream id.
        /// </summary>
        internal long StreamId
        {
            get
            {
                return (uint) this.HighId << 32 | (uint) this.LowId;
            }
        }

        public int HighId;
        public int LowId;

        public bool IsAccepted;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyJoinRequestMessage"/> class.
        /// </summary>
        public ReplyJoinRequestMessage()
        {
            // ReplyJoinRequestMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyJoinRequestMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ReplyJoinRequestMessage(ByteStream Stream) : base(Stream)
        {
            // ReplyJoinRequestMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.HighId     = this.Stream.ReadInt();
            this.LowId      = this.Stream.ReadInt();
            this.IsAccepted = this.Stream.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.HighId);
            this.Stream.WriteInt(this.LowId);
            this.Stream.WriteBoolean(this.IsAccepted);
        }
    }
}