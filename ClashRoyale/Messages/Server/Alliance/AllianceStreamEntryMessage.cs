namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
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
        public int EntryType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntryMessage"/> class.
        /// </summary>
        public AllianceStreamEntryMessage()
        {
            // AllianceStreamEntryMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntryMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AllianceStreamEntryMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceStreamEntryMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntryMessage"/> class.
        /// </summary>
        /// <param name="Entry">The entry.</param>
        public AllianceStreamEntryMessage(StreamEntry Entry)
        {
            this.EntryType   = Entry.Type;
            this.StreamEntry = Entry;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.EntryType = this.Stream.ReadVInt();
            this.StreamEntry.Decode(this.Stream);
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.EntryType);
            this.StreamEntry.Encode(this.Stream);
        }
    }
}