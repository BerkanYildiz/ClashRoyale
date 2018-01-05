namespace ClashRoyale.Messages.Server.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Logic.Scoring.Entries;

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

        private readonly LeaderboardClans Leaderboard;
        private readonly AllianceRankingEntry[] AllianceRankingList;
        private readonly AllianceRankingEntry[] PreviousSeasonTopAlliances;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Leaderboard">The leaderboard.</param>
        public AllianceRankingListMessage(Device Device, LeaderboardClans Leaderboard) : base(Device)
        {
            this.Leaderboard                = Leaderboard;
            this.AllianceRankingList        = Leaderboard.Clans.ToArray();
            this.PreviousSeasonTopAlliances = Leaderboard.LastSeason.ToArray();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.AllianceRankingList.Length);

            for (int I = 0; I < this.AllianceRankingList.Length; I++)
            {
                this.AllianceRankingList[I].Encode(this.Stream);
            }
        }
    }
}