namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Logic.Scoring.Entries;
    using ClashRoyale.Messages;

    internal class AvatarRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 29733;
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

        private readonly LeaderboardPlayers Leaderboard;
        private readonly AvatarRankingEntry[] AvatarRankingList;
        private readonly AvatarRankingEntry[] PreviousSeasonTopPlayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Leaderboard">The leaderboard.</param>
        public AvatarRankingListMessage(Device Device, LeaderboardPlayers Leaderboard) : base(Device)
        {
            this.Leaderboard                = Leaderboard;
            this.AvatarRankingList          = Leaderboard.Players.ToArray();
            this.PreviousSeasonTopPlayers   = Leaderboard.LastSeason.ToArray();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            if (this.AvatarRankingList != null)
            {
                this.Stream.WriteVInt(this.AvatarRankingList.Length);

                for (int I = 0; I < this.AvatarRankingList.Length; I++)
                {
                    this.AvatarRankingList[I].Encode(this.Stream);
                }
            }
            else
            {
                this.Stream.WriteVInt(-1);
            }

            if (this.PreviousSeasonTopPlayers != null)
            {
                this.Stream.WriteInt(this.PreviousSeasonTopPlayers.Length);

                for (int I = 0; I < this.PreviousSeasonTopPlayers.Length; I++)
                {
                    this.PreviousSeasonTopPlayers[I].Encode(this.Stream);
                }
            }
            else
            {
                this.Stream.WriteVInt(-1);
            }

            this.Stream.WriteInt((int) this.Leaderboard.TimeLeft.TotalSeconds);
        }
    }
}