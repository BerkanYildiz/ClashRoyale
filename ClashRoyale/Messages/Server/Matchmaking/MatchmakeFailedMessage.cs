namespace ClashRoyale.Messages.Server.Matchmaking
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class MatchmakeFailedMessage : Message
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
        public MatchmakeFailedMessage()
        {
            // MatchmakeFailedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchmakeFailedMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public MatchmakeFailedMessage(ByteStream Stream) : base(Stream)
        {
            // MatchmakeFailedMessage.
        }
    }
}