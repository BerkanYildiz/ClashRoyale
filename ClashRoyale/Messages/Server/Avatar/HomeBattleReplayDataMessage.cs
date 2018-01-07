namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;

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
            // TODO : Decode.
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