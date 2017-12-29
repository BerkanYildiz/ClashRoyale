namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;

    internal class BattleReportStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20032;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleReportStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public BattleReportStreamMessage(Device Device) : base(Device)
        {
            // BattleReportStreamMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteLong(this.Device.GameMode.Player.PlayerId);
            this.Stream.WriteVInt(0); // Count
        }
    }
}