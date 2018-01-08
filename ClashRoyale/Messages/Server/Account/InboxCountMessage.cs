namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class InboxCountMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 26068;
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

        public int InboxNewMessageCnt;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxCountMessage"/> class.
        /// </summary>
        public InboxCountMessage()
        {
            // InboxCountMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxCountMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public InboxCountMessage(ByteStream Stream) : base(Stream)
        {
            // InboxCountMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxCountMessage"/> class.
        /// </summary>
        /// <param name="NewMessageCount">The new message count.</param>
        public InboxCountMessage(int NewMessageCount)
        {
            this.InboxNewMessageCnt = NewMessageCount;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.InboxNewMessageCnt = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.InboxNewMessageCnt);
        }
    }
}