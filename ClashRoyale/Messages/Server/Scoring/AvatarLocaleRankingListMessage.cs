namespace ClashRoyale.Messages.Server.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Scoring;

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

        public AvatarRankingEntry[] AvatarRankingList;
        public AvatarRankingEntry[] PreviousSeasonTopPlayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarLocaleRankingListMessage"/> class.
        /// </summary>
        public AvatarLocaleRankingListMessage()
        {
            // AvatarLocaleRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarLocaleRankingListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public AvatarLocaleRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AvatarLocaleRankingListMessage.
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