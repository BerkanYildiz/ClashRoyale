namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Battle.Event;
    using ClashRoyale.Messages;

    internal class SendBattleEventMessage : Message
    {
        internal BattleEvent Event;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12951;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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
        public override void Decode()
        {
            this.Event = new BattleEvent();
            this.Event.Decode(this.Stream);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            if (this.Device.GameMode.State == HomeState.Attack)
            {
                this.Device.GameMode.SectorManager.ReceiveBattleEvent(this.Event);
            }
            else
            {
                Logging.Error(this.GetType(), "State != HomeState.Attack at Process().");
            }
        }
    }
}