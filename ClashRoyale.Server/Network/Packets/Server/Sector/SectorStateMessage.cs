namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Server.Logic;

    internal class SectorStateMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 21903;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Sector;
            }
        }

        private readonly byte[] FullUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorStateMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Update">The update.</param>
        public SectorStateMessage(Device Device, byte[] Update) : base(Device)
        {
            this.FullUpdate = Update;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.AddRange(ZLibHelper.CompressCompressableByteArray(this.FullUpdate));
        }
    }
}