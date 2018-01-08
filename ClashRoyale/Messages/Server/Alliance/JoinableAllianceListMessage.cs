namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Alliance.Entries;

    public class JoinableAllianceListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24304;
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

        public AllianceHeaderEntry[] Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListMessage"/> class.
        /// </summary>
        public JoinableAllianceListMessage()
        {
            // JoinableAllianceListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public JoinableAllianceListMessage(ByteStream Stream) : base(Stream)
        {
            // JoinableAllianceListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        public JoinableAllianceListMessage(AllianceHeaderEntry[] Entries)
        {
            this.Entries = Entries;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Entries = new AllianceHeaderEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.Entries.Length; i++)
            {
                AllianceHeaderEntry Entry = new AllianceHeaderEntry();
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

            foreach (AllianceHeaderEntry Entry in this.Entries)
            {
                Entry.Encode(this.Stream);
            }
        }
    }
}