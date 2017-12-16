namespace ClashRoyale.Server.Network.Packets.Server.Avatar
{
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class BattleResultMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20225;
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

        private readonly byte[] FullUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleResultMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Update">The update.</param>
        public BattleResultMessage(Device Device, byte[] Update) : base(Device)
        {
            this.FullUpdate = Update;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
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