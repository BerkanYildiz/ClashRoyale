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
        
        public AllianceRankingEntry[] AllianceRankingList;
        public AllianceRankingEntry[] PreviousSeasonTopAlliances;

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
        /// <param name="Device">The device.</param>
        /// <param name="Leaderboard">The leaderboard.</param>
        public AllianceLocalRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceLocalRankingListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.AllianceRankingList = new AllianceRankingEntry[this.Stream.ReadInt()];

            for (int i = 0; i < this.AllianceRankingList.Length; i++)
            {
                AllianceRankingEntry Entry = new AllianceRankingEntry();
                Entry.Decode(this.Stream);
                this.AllianceRankingList[i] = Entry;
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.AllianceRankingList.Length);

            for (int I = 0; I < this.AllianceRankingList.Length; I++)
            {
                this.AllianceRankingList[I].Encode(this.Stream);
            }
        }
    }
}