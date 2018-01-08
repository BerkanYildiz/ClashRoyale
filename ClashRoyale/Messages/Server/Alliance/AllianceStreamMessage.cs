namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Alliance.Stream;

    public class AllianceStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24719;
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

        public StreamEntry[] Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        public AllianceStreamMessage()
        {
            // AllianceStreamMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AllianceStreamMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceStreamMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        public AllianceStreamMessage(StreamEntry[] Entries)
        {
            this.Entries = Entries;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Entries = new StreamEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.Entries.Length; i++)
            {
                int EntryType = this.Stream.ReadVInt();

                StreamEntry Entry = new StreamEntry();
                Entry.Decode(this.Stream);
                this.Entries[i] = Entry;
            }
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Entries.Length);

            for (int i = 0; i < this.Entries.Length; i++)
            {
                this.Stream.WriteVInt(this.Entries[i].Type);
                this.Entries[i].Encode(this.Stream);
            }
        }
    }
}