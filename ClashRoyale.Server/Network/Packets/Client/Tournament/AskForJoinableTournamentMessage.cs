namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForJoinableTournamentMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 16103;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        internal override void Decode()
        {
            this.Stream.ReadVInt();
            this.Stream.ReadVInt();

            this.Stream.ReadBoolean();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.SendMessage(new JoinableTournamentListMessage(this.Device));
        }
    }
}