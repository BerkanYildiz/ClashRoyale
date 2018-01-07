namespace ClashRoyale.Messages.Client.Matchmaking
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class CancelMatchmakeMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12269;
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
        /// Initializes a new instance of the <see cref="CancelMatchmakeMessage"/> class.
        /// </summary>
        public CancelMatchmakeMessage()
        {
            // CancelMatchmakeMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelMatchmakeMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public CancelMatchmakeMessage(ByteStream Stream) : base(Stream)
        {
            // CancelMatchmakeMessage.
        }
    }
}