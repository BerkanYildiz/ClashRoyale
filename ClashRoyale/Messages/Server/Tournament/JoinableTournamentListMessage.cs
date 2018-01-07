namespace ClashRoyale.Messages.Server.Tournament
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class JoinableTournamentListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 26108;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableTournamentListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public JoinableTournamentListMessage(ByteStream Stream) : base(Stream)
        {
            // Joinable_Tournaments_Message.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(0);
        }
    }
}