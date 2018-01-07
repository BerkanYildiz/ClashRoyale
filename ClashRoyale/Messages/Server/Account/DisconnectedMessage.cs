namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class DisconnectedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 25892;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        public bool ShowPopup;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedMessage"/> class.
        /// </summary>
        public DisconnectedMessage()
        {
            // DisconnectedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public DisconnectedMessage(ByteStream Stream) : base(Stream)
        {
            // DisconnectedMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ShowPopup = this.Stream.ReadBoolean();
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(this.ShowPopup);
        }
    }
}