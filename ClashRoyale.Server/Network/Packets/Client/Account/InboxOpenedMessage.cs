namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class InboxOpenedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10905;
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
        /// Initializes a new instance of the <see cref="InboxOpenedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public InboxOpenedMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // InboxOpenedMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.SendMessage(new InboxListMessage(this.Device));
        }
    }
}