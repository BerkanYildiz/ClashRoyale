namespace ClashRoyale.Messages.Client.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class VisitHomeMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 19860;
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

        public int HighId;
        public int LowId;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitHomeMessage"/> class.
        /// </summary>
        public VisitHomeMessage()
        {
            // VisitHomeMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitHomeMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public VisitHomeMessage(ByteStream Stream) : base(Stream)
        {
            // VisitHomeMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.HighId = this.Stream.ReadInt();
            this.LowId  = this.Stream.ReadInt();
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.HighId);
            this.Stream.WriteInt(this.LowId);
        }
    }
}