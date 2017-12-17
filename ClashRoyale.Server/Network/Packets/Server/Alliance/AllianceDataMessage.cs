namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Enums;

    internal class AllianceDataMessage : Message
    {
        internal Clan Clan;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24301;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Clan">The alliance.</param>
        public AllianceDataMessage(Device Device, Clan Clan) : base(Device)
        {
            this.Clan = Clan;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        internal override void Encode()
        {
            this.Clan.Encode(this.Stream);
        }
    }
}