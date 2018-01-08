namespace ClashRoyale.Messages.Server.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Scoring;

    public class AllianceLocalRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 26973;
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
        /// Initializes a new instance of the <see cref="AllianceLocalRankingListMessage"/> class.
        /// </summary>
        public AllianceLocalRankingListMessage()
        {
            // AllianceLocalRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceLocalRankingListMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AllianceLocalRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceLocalRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceLocalRankingListMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        /// <param name="LastSeasonEntries">The last season entries.</param>
        public AllianceLocalRankingListMessage(AllianceRankingEntry[] Entries, AllianceRankingEntry[] LastSeasonEntries)
        {
            this.Entries = Entries;
            this.LastSeasonEntries = LastSeasonEntries;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Entries = new AllianceRankingEntry[this.Stream.ReadInt()];

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
            this.Stream.WriteInt(this.Entries.Length);

            foreach (AllianceRankingEntry Entry in this.Entries)
            {
                Entry.Encode(this.Stream);
            }
        }
    }
}