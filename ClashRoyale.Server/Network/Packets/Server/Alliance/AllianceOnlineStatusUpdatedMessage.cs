namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class AllianceOnlineStatusUpdatedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20207;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.MemberOnline);
            this.Stream.WriteVInt(0); // Array
        }
    }
}