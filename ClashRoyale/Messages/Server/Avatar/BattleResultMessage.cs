namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;

    public class BattleResultMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20225;
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

        public byte[] FullUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleResultMessage"/> class.
        /// </summary>
        /// <param name="Update">The update.</param>
        public BattleResultMessage(byte[] Update)
        {
            this.FullUpdate = Update;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(3); // Crown
            this.Stream.WriteVInt(99); // Trophies Gain
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);

            this.Stream.WriteVInt(0);
            this.Stream.WriteBytes(ZLibHelper.CompressCompressableByteArray(this.FullUpdate));
            this.Stream.WriteBoolean(false);

            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);

            this.Stream.WriteBoolean(false);
        }
    }
}