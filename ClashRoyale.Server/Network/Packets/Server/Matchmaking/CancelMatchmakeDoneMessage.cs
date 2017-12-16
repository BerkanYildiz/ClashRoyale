namespace ClashRoyale.Server.Network.Packets.Server.Matchmaking
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class CancelMatchmakeDoneMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24125;
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
        /// Initializes a new instance of the <see cref="CancelMatchmakeDoneMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public CancelMatchmakeDoneMessage(Device Device) : base(Device)
        {
            // CancelMatchmakeDoneMessage.
        }
    }
}