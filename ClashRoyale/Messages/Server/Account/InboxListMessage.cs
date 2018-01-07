namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Inbox;

    public class InboxListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24445;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxListMessage"/> class.
        /// </summary>
        public InboxListMessage()
        {
            // InboxListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="InboxEntries">The inbox entries.</param>
        public InboxListMessage(ByteStream Stream) : base(Stream)
        {
            // InboxListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            InboxManager.Decode(this.Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            InboxManager.Encode(this.Stream);
        }
    }
}