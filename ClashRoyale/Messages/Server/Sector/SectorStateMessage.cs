namespace ClashRoyale.Messages.Server.Sector
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;

    public class SectorStateMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 21873;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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
        /// <param name="Update">The update.</param>
        public SectorStateMessage(byte[] Update)
        {
            this.FullUpdate = Update;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.AddRange(ZLibHelper.CompressCompressableByteArray(this.FullUpdate));
        }
    }
}