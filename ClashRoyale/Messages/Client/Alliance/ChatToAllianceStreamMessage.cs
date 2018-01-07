namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class ChatToAllianceStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14308;
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

        public string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatToAllianceStreamMessage"/> class.
        /// </summary>
        public ChatToAllianceStreamMessage()
        {
            // ChatToAllianceStreamMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatToAllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ChatToAllianceStreamMessage(ByteStream Stream) : base(Stream)
        {
            // ChatToAllianceStreamMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Message = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Message);
        }
    }
}