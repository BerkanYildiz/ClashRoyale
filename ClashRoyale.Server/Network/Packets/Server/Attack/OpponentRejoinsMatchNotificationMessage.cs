namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;

    internal class OpponentRejoinsMatchNotificationMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20802;
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
        /// Initializes a new instance of the <see cref="OpponentRejoinsMatchNotificationMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public OpponentRejoinsMatchNotificationMessage(Device Device) : base(Device)
        {
            // Opponent_Rejoins_Match_Notification_Message.
        }
    }
}