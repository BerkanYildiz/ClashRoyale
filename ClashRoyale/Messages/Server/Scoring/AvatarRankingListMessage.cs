namespace ClashRoyale.Messages.Server.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Scoring;

    public class AvatarRankingListMessage : Message
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
        
        public AvatarRankingEntry[] AvatarRankingList;
        public AvatarRankingEntry[] PreviousSeasonTopPlayers;

        public int TimeLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingListMessage"/> class.
        /// </summary>
        public AvatarRankingListMessage()
        {
            // AvatarRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Leaderboard">The leaderboard.</param>
        public AvatarRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AvatarRankingListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.AvatarRankingList = new AvatarRankingEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.AvatarRankingList.Length; i++)
            {
                AvatarRankingEntry Entry = new AvatarRankingEntry();
                Entry.Decode(this.Stream);
                this.AvatarRankingList[i] = Entry;
            }

            this.PreviousSeasonTopPlayers = new AvatarRankingEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.PreviousSeasonTopPlayers.Length; i++)
            {
                AvatarRankingEntry Entry = new AvatarRankingEntry();
                Entry.Decode(this.Stream);
                this.PreviousSeasonTopPlayers[i] = Entry;
            }

            this.TimeLeft = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.AvatarRankingList.Length);

            foreach (AvatarRankingEntry Entry in this.AvatarRankingList)
            {
                Entry.Encode(this.Stream);
            }

            this.Stream.WriteInt(this.PreviousSeasonTopPlayers.Length);

            foreach (AvatarRankingEntry Entry in this.PreviousSeasonTopPlayers)
            {
                Entry.Encode(this.Stream);
            }

            this.Stream.WriteInt(this.TimeLeft);
        }
    }
}