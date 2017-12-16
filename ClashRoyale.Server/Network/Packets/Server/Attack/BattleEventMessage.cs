namespace ClashRoyale.Server.Network.Packets.Server.Attack
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Event;

    internal class BattleEventMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 22952;
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

        private readonly BattleEvent Event;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEventMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Event">The event.</param>
        public BattleEventMessage(Device Device, BattleEvent Event) : base(Device)
        {
            this.Event = Event;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        internal override void Encode()
        {
            this.Event.Encode(this.Stream);
        }
    }
}