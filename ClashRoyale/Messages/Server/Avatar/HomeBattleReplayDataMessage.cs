namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

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

        public byte[] CompressedReplayDataJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayDataMessage"/> class.
        /// </summary>
        public HomeBattleReplayDataMessage()
        {
            // HomeBattleReplayDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public HomeBattleReplayDataMessage(ByteStream Stream) : base(Stream)
        {
            // HomeBattleReplayDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayDataMessage"/> class.
        /// </summary>
        /// <param name="CompressedReplayData">The compressed replay data.</param>
        public HomeBattleReplayDataMessage(byte[] CompressedReplayData)
        {
            this.CompressedReplayDataJson = CompressedReplayData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            int Length = this.Stream.ReadVInt();
            this.CompressedReplayDataJson = this.Stream.ReadBytes();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.CompressedReplayDataJson.Length);
            this.Stream.WriteBytes(this.CompressedReplayDataJson);
        }
    }
}