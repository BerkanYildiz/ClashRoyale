namespace ClashRoyale.Server.Logic.Scoring.Entries
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Player;

    internal class AvatarRankingEntry : RankingEntry
    {
        private long HomeId;
        private long AllianceId;

        private int ExpLevel;

        private string Region;
        private string AllianceName;

        private ArenaData Arena;
        private AllianceBadgeData Badge;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingEntry"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        internal AvatarRankingEntry(Player Player)
        {
            this.Initialize(Player);
        }

        /// <summary>
        /// Initializes the specified player.
        /// </summary>
        /// <param name="Player">The player.</param>
        internal void Initialize(Player Player)
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
        /// Determines whether the specified scored player is better.
        /// </summary>
        /// <param name="ScoredPlayer">The scored player.</param>
        /// <returns>
        ///   <c>true</c> if the specified scored player is better; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsBetter(AvatarRankingEntry ScoredPlayer)
        {
            return this.Score > ScoredPlayer.Score;
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal void Encode(ByteStream Stream)
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
    }
}