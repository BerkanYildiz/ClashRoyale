namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;

    internal class KeepAliveServerMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24135;
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
        /// Initializes a new instance of the <see cref="KeepAliveServerMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public KeepAliveServerMessage(Device Device) : base(Device)
        {
            // KeepAliveServerMessage.
        }
    }
}