namespace ClashRoyale.Messages.Server.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Logic.Scoring.Entries;

    public class AvatarLocaleRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 25390;
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

        private readonly AvatarRankingEntry[] AvatarRankingList;
        private readonly AvatarRankingEntry[] PreviousSeasonTopPlayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarLocaleRankingListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Leaderboard">The leaderboard.</param>
        public AvatarLocaleRankingListMessage(Device Device, LeaderboardPlayers Leaderboard) : base(Device)
        {
            this.AvatarRankingList          = Leaderboard.Players.ToArray();
            this.PreviousSeasonTopPlayers   = Leaderboard.LastSeason.ToArray();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.AvatarRankingList.Length);

            for (int I = 0; I < this.AvatarRankingList.Length; I++)
            {
                this.AvatarRankingList[I].Encode(this.Stream);
            }

            this.Stream.WriteInt(this.PreviousSeasonTopPlayers.Length);

            for (int I = 0; I < this.PreviousSeasonTopPlayers.Length; I++)
            {
                this.PreviousSeasonTopPlayers[I].Encode(this.Stream);
            }
        }
    }
}