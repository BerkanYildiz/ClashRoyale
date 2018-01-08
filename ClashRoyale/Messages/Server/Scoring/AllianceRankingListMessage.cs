namespace ClashRoyale.Messages.Server.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Scoring;

    public class AllianceRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 25105;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Scoring;
            }
        }

        public AllianceRankingEntry[] Entries;
        public AllianceRankingEntry[] LastSeasonEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingListMessage"/> class.
        /// </summary>
        public AllianceRankingListMessage()
        {
            // AllianceRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingListMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AllianceRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingListMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        /// <param name="LastSeasonEntries">The last season entries.</param>
        public AllianceRankingListMessage(AllianceRankingEntry[] Entries, AllianceRankingEntry[] LastSeasonEntries)
        {
            this.Entries = Entries;
            this.LastSeasonEntries = LastSeasonEntries;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Entries = new AllianceRankingEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.Entries.Length; i++)
            {
                AllianceRankingEntry Entry = new AllianceRankingEntry();
                Entry.Decode(this.Stream);
                this.Entries[i] = Entry;
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Entries.Length);

            foreach (AllianceRankingEntry Entry in this.Entries)
            {
                Entry.Encode(this.Stream);
            }
        }
    }
}