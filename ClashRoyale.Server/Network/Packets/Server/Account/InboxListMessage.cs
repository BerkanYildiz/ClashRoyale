namespace ClashRoyale.Server.Network.Packets.Server.Account
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class InboxListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24445;
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
        /// Initializes a new instance of the <see cref="InboxListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="InboxEntries">The inbox entries.</param>
        public InboxListMessage(Device Device) : base(Device)
        {
            // InboxListMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            InboxManager.Encode(this.Stream);
        }
    }
}