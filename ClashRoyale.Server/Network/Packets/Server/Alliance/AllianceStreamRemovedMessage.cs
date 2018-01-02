namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class AllianceStreamRemovedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24312;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private readonly long StreamEntryId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamRemovedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="StreamEntryId">The stream entry identifier.</param>
        public AllianceStreamRemovedMessage(Device Device, long StreamEntryId) : base(Device)
        {
            this.StreamEntryId = StreamEntryId;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteLong(this.StreamEntryId);
        }
    }
}