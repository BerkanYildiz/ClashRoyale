namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;

    internal class OpponentLeftMatchNotificationMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20801;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpponentLeftMatchNotificationMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public OpponentLeftMatchNotificationMessage(Device Device) : base(Device)
        {
            // OpponentLeftMatchNotificationMessage.
        }
    }
}