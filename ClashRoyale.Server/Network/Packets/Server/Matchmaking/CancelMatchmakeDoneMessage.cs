namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class CancelMatchmakeDoneMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20817;
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
        /// Initializes a new instance of the <see cref="CancelMatchmakeDoneMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public CancelMatchmakeDoneMessage(Device Device) : base(Device)
        {
            // CancelMatchmakeDoneMessage.
        }
    }
}