namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;

    public class HomeBattleReplayDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 25412;
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
        public override void Encode()
        {
            this.Stream.WriteVInt(this.CompressedReplayDataJson.Length);
            this.Stream.WriteBytes(this.CompressedReplayDataJson);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            this.Device.GameMode.EndHomeState();
        }
    }
}