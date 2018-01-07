namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance.Stream;

    public class AllianceStreamEntryMessage : Message
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

        public StreamEntry StreamEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntryMessage"/> class.
        /// </summary>
        /// <param name="Entry">The entry.</param>
        public AllianceStreamEntryMessage(StreamEntry Entry)
        {
            this.StreamEntry = Entry;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.StreamEntry.Type);
            this.StreamEntry.Encode(this.Stream);
        }
    }
}