namespace ClashRoyale.Messages.Client.Tournament
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForJoinableTournamentMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 16103;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Tournament;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForJoinableTournamentMessage"/> class.
        /// </summary>
        public AskForJoinableTournamentMessage()
        {
            // AskForJoinableTournamentMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForJoinableTournamentMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForJoinableTournamentMessage(ByteStream Stream) : base(Stream)
        {
            // AskForJoinableTournamentMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Stream.ReadVInt();
            this.Stream.ReadVInt();
            this.Stream.ReadBoolean();
        }
    }
}