namespace ClashRoyale.Server.Network.Packets.Server.Home
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class MatchmakeInfoMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24107;
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

        private readonly int EstimedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchmakeInfoMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="EstimedTime">The estimed time.</param>
        public MatchmakeInfoMessage(Device Device, int EstimedTime) : base(Device)
        {
            this.EstimedTime = EstimedTime;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteInt(this.EstimedTime);
        }
    }
}