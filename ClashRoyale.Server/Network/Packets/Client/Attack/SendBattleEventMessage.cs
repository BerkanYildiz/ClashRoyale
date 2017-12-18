namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Battle.Event;

    internal class SendBattleEventMessage : Message
    {
        internal BattleEvent Event;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 12951;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBattleEventMessage"/> class.
        /// </summary>
        public SendBattleEventMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // SendBattleEventMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Event = new BattleEvent();
            this.Event.Decode(this.Stream);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (this.Device.GameMode.State == HomeState.Attack)
            {
                this.Device.GameMode.SectorManager.ReceiveBattleEvent(this.Event);
            }
            else
            {
                Logging.Error(this.GetType(), "Player sent a BattleEvent but is not in a battle, therefore this message shouldn't be sent.");
            }
        }
    }
}