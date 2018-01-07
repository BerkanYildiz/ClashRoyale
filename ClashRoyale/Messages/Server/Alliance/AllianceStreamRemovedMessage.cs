namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;

    public class AllianceStreamRemovedMessage : Message
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

        public long StreamEntryId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamRemovedMessage"/> class.
        /// </summary>
        /// <param name="StreamEntryId">The stream entry identifier.</param>
        public AllianceStreamRemovedMessage(long StreamEntryId)
        {
            this.StreamEntryId = StreamEntryId;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.StreamEntryId = this.Stream.ReadLong();
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