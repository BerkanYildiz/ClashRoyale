namespace ClashRoyale.Server.Network.Packets.Server.Account
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class InboxCountMessage : Message
    {
        internal int InboxNewMessageCnt;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24447;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxCountMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public InboxCountMessage(Device Device) : base(Device)
        {
            // InboxCountMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.InboxNewMessageCnt = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.InboxNewMessageCnt);
        }
    }
}