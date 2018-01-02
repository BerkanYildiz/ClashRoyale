namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForBattleReplayStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 15827;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        private LogicLong PlayerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForBattleReplayStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForBattleReplayStreamMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForBattleReplayStreamMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.PlayerId = this.Stream.DecodeLogicLong();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            this.Device.NetworkManager.SendMessage(new BattleReportStreamMessage(this.Device));
        }
    }
}