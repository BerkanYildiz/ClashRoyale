namespace ClashRoyale.Logic.Scoring
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Player;

    public class AvatarRankingEntry : RankingEntry
    {
        public long HomeId;
        public long AllianceId;

        public int ExpLevel;

        public string Region;
        public string AllianceName;

        public ArenaData Arena;
        public AllianceBadgeData Badge;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingEntry"/> class.
        /// </summary>
        public AvatarRankingEntry()
        {
            // AvatarRankingEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingEntry"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        public AvatarRankingEntry(Player Player)
        {
            this.Initialize(Player);
        }

        /// <summary>
        /// Initializes the specified player.
        /// </summary>
        /// <param name="Player">The player.</param>
        public void Initialize(Player Player)
        {
            base.Initialize(Player.PlayerId, Player.Name, Player.Score, 1, 1);

            this.Name       = Player.Name;
            this.Score      = Player.Score;

            this.HomeId    = Player.Home.HomeId;
            this.ExpLevel  = Player.ExpLevel;
            this.Arena     = Player.Arena;

            if (Player.AccountLocation != null)
            {
                this.Region = Player.AccountLocation.Name;
            }

            if (Player.IsInAlliance)
            {
                this.AllianceId    = Player.AllianceId;
                this.AllianceName  = Player.AllianceName;
                this.Badge         = Player.Badge;
            }
            else if (this.AllianceId > 0)
            {
                this.AllianceId    = 0;
                this.AllianceName  = null;
                this.Badge         = null;
            }
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.ExpLevel = Stream.ReadInt();

            Stream.ReadInt();
            Stream.ReadInt();
            Stream.ReadInt();
            Stream.ReadInt();
            Stream.ReadInt();

            this.Region = Stream.ReadString();
            this.HomeId = Stream.ReadLong();

            Stream.ReadInt();
            Stream.ReadInt();

            if (Stream.ReadBoolean())
            {
                this.AllianceId     = Stream.ReadLong();
                this.AllianceName   = Stream.ReadString();
                this.Badge          = Stream.DecodeData<AllianceBadgeData>();
            }

            if (Stream.ReadBoolean())
            {
                this.Arena          = Stream.DecodeData<ArenaData>();
            }
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteInt(this.ExpLevel);

            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);

            Stream.WriteString(this.Region);

            Stream.WriteLong(this.HomeId);

            Stream.WriteInt(50);
            Stream.WriteInt(0);

            Stream.WriteBoolean(this.AllianceId != 0);

            if (this.AllianceId != 0)
            {
                Stream.WriteLong(this.AllianceId);
                Stream.WriteString(this.AllianceName);
                Stream.EncodeData(this.Badge);
            }

            Stream.WriteBoolean(this.Arena != null);

            if (this.Arena != null)
            {
                Stream.EncodeData(this.Arena);
            }
        }

        /// <summary>
        /// Determines whether the specified scored player is better.
        /// </summary>
        /// <param name="ScoredPlayer">The scored player.</param>
        /// <returns>
        ///   <c>true</c> if the specified scored player is better; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBetter(AvatarRankingEntry ScoredPlayer)
        {
            return this.Score > ScoredPlayer.Score;
        }
    }
}