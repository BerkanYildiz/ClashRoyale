namespace ClashRoyale.Logic.Scoring
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Alliance.Entries;

    public class AllianceRankingEntry : RankingEntry
    {
        public AllianceBadgeData BadgeData;
        public RegionData RegionData;

        public int MembersCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingEntry"/> class.
        /// </summary>
        public AllianceRankingEntry()
        {
            // AllianceRankingEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingEntry"/> class.
        /// </summary>
        /// <param name="HeaderEntry">The header entry.</param>
        public AllianceRankingEntry(AllianceHeaderEntry HeaderEntry)
        {
            this.Initialize(HeaderEntry);
        }

        /// <summary>
        /// Initializes the specified alliance.
        /// </summary>
        /// <param name="Clan">The alliance.</param>
        public void Initialize(AllianceHeaderEntry HeaderEntry)
        {
            base.Initialize(HeaderEntry.HighId, HeaderEntry.Name, HeaderEntry.Score, 1, 1);

            this.RegionData     = HeaderEntry.Region;
            this.BadgeData      = HeaderEntry.Badge;
            this.MembersCount   = HeaderEntry.MembersCount;
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.BadgeData      = Stream.DecodeData<AllianceBadgeData>();
            this.RegionData     = Stream.DecodeData<RegionData>();
            this.MembersCount   = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.EncodeData(this.BadgeData);
            Stream.EncodeData(this.RegionData);
            Stream.WriteVInt(this.MembersCount);
        }
        
        /// <summary>
        /// Determines whether the specified scored clan is better.
        /// </summary>
        /// <param name="ScoredClan">The scored clan.</param>
        /// <returns>
        ///   <c>true</c> if the specified scored clan is better; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBetter(AllianceRankingEntry ScoredClan)
        {
            return this.Score > ScoredClan.Score;
        }
    }
}