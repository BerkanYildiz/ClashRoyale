namespace ClashRoyale.Messages.Client.Tournament
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages.Server.Tournament;

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
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForJoinableTournamentMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
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

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            this.Device.NetworkManager.SendMessage(new JoinableTournamentListMessage(this.Device));
        }
    }
}