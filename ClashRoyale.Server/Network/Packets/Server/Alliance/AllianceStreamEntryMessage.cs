namespace ClashRoyale.Server.Network.Packets.Server.Alliance
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Stream;

    internal class AllianceStreamEntryMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24312;
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

        private readonly StreamEntry StreamEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntryMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Entry">The entry.</param>
        public AllianceStreamEntryMessage(Device Device, StreamEntry Entry) : base(Device)
        {
            this.StreamEntry = Entry;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.StreamEntry.Type);
            this.StreamEntry.Encode(this.Stream);
        }
    }
}