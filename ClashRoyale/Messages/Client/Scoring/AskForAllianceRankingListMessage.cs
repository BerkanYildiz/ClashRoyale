namespace ClashRoyale.Messages.Client.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Maths;

    public class AskForAllianceRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14171;
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

        public LogicLong PlayerId;
        public bool IsLocal;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAllianceRankingListMessage"/> class.
        /// </summary>
        public AskForAllianceRankingListMessage()
        {
            // AskForAllianceRankingListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAllianceRankingListMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForAllianceRankingListMessage(ByteStream Stream) : base(Stream)
        {
            // AskForAllianceRankingListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.IsLocal = this.Stream.ReadBoolean();

            if (this.Stream.ReadBoolean())
            {
                this.PlayerId = this.Stream.ReadLong();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(this.IsLocal);
            this.Stream.WriteBoolean(!this.PlayerId.IsZero);

            if (!this.PlayerId.IsZero)
            {
                this.Stream.WriteLong(this.PlayerId);
            }
        }
    }
}