namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;

    internal class HomeBattleReplayDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24114;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        private readonly byte[] CompressedReplayDataJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="CompressedReplayData">The compressed replay data.</param>
        public HomeBattleReplayDataMessage(Device Device, byte[] CompressedReplayData) : base(Device)
        {
            this.CompressedReplayDataJson = CompressedReplayData;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.CompressedReplayDataJson.Length);
            this.Stream.WriteBytes(this.CompressedReplayDataJson);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.GameMode.EndHomeState();
        }
    }
}