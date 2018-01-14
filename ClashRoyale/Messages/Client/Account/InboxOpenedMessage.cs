namespace ClashRoyale.Messages.Client.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Messages;

    public class InboxOpenedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10517;
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
        /// Initializes a new instance of the <see cref="InboxOpenedMessage"/> class.
        /// </summary>
        public InboxOpenedMessage()
        {
            // InboxOpenedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxOpenedMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public InboxOpenedMessage(ByteStream Stream) : base(Stream)
        {
            // InboxOpenedMessage.
        }
    }
}