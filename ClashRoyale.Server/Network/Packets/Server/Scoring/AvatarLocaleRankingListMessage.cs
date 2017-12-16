namespace ClashRoyale.Server.Network.Packets.Server.Scoring
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Scoring;
    using ClashRoyale.Server.Logic.Scoring.Entries;

    internal class AvatarLocaleRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24404;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        internal override void Encode()
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