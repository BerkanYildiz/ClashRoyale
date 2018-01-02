namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class AllianceOnlineStatusUpdatedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24457;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private readonly int MemberOnline;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceOnlineStatusUpdatedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="MemberOnline">The member online.</param>
        public AllianceOnlineStatusUpdatedMessage(Device Device, int MemberOnline) : base(Device)
        {
            this.MemberOnline = MemberOnline;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.MemberOnline);
            this.Stream.WriteVInt(0); // Array
        }
    }
}