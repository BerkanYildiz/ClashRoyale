namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Maths;

    internal class AskForAvatarRankingListMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 11149;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Scoring;
            }
        }

        private LogicLong AccountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarRankingListMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        public AskForAvatarRankingListMessage(Bot Bot) : base(Bot)
        {
            // AskForAvatarRankingListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Stream.ReadBoolean();

            if (this.Stream.ReadBoolean())
            {
                this.AccountId = this.Stream.ReadLong();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteBoolean(false);
            this.Stream.WriteBoolean(!this.AccountId.IsZero);

            if (this.AccountId.IsZero == false)
            {
                this.Stream.WriteLong(this.AccountId);
            }
        }
    }
}