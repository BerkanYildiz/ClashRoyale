namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class MatchmakeFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24108;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Matchmaking;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchmakeFailedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public MatchmakeFailedMessage(Device Device) : base(Device)
        {
            // MatchmakeFailedMessage.
        }
    }
}