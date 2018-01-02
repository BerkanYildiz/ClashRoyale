namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class BattleReportStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20032;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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
        public override void Encode()
        {
            this.Stream.WriteLong(this.Device.GameMode.Player.PlayerId);
            this.Stream.WriteVInt(0); // Count
        }
    }
}