namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;

    internal class KeepAliveMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 19911;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public KeepAliveMessage(Device Device) : base(Device)
        {
            // KeepAliveMessage.
        }
    }
}