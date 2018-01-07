namespace ClashRoyale.Messages.Server.Matchmaking
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class CancelMatchmakeDoneMessage : Message
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
        public CancelMatchmakeDoneMessage()
        {
            // CancelMatchmakeDoneMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelMatchmakeDoneMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public CancelMatchmakeDoneMessage(ByteStream Stream) : base(Stream)
        {
            // CancelMatchmakeDoneMessage.
        }
    }
}