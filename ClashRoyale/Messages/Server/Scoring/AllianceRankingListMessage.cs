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

        public AllianceRankingEntry[] AllianceRankingList;
        public AllianceRankingEntry[] PreviousSeasonTopAlliances;

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
        /// <param name="Device">The device.</param>
        public AllianceRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceRankingListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.AllianceRankingList = new AllianceRankingEntry[this.Stream.ReadVInt()];

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
            this.Stream.WriteVInt(this.AllianceRankingList.Length);

            foreach (AllianceRankingEntry Entry in this.AllianceRankingList)
            {
                Entry.Encode(this.Stream);
            }
        }
    }
}