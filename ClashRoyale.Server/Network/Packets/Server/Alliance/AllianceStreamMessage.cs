namespace ClashRoyale.Server.Network.Packets.Server.Alliance
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Stream;

    internal class AllianceStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24311;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private readonly StreamEntry[] Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Entries">The entries.</param>
        public AllianceStreamMessage(Device Device, StreamEntry[] Entries) : base(Device)
        {
            this.Entries = Entries;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.Entries.Length);

            for (int I = 0; I < this.Entries.Length; I++)
            {
                this.Stream.WriteVInt(this.Entries[I].Type);
                this.Entries[I].Encode(this.Stream);
            }
        }
    }
}