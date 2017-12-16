namespace ClashRoyale.Server.Network.Packets.Server.Home
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class BattleReportStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24413;
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