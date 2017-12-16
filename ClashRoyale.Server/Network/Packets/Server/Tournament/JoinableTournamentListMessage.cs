namespace ClashRoyale.Server.Network.Packets.Server.Tournament
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class JoinableTournamentListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 26108;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        public JoinableTournamentListMessage(Device Device) : base(Device)
        {
            // Joinable_Tournaments_Message.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(0);
        }
    }
}