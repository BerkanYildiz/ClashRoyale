namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class MatchmakeFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24108;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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