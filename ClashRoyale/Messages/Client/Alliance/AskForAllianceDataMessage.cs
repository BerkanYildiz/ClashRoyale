namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForAllianceDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10609;
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
        /// Initializes a new instance of the <see cref="AskForAllianceDataMessage"/> class.
        /// </summary>
        public AskForAllianceDataMessage()
        {
            // AskForAllianceDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAllianceDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForAllianceDataMessage(ByteStream Stream) : base(Stream)
        {
            // AskForAllianceDataMessage.
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