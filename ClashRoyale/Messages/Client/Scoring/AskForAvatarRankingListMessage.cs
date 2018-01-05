namespace ClashRoyale.Messages.Client.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages.Server.Scoring;

    public class AskForAvatarRankingListMessage : Message
    {
        internal LogicLong AccountId;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 11149;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarRankingListMessage"/> class.
        /// </summary>
        public AskForAvatarRankingListMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForAvatarRankingListMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
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
        public override void Encode()
        {
            this.Stream.WriteBoolean(false);
            this.Stream.WriteBoolean(!this.AccountId.IsZero);

            if (!this.AccountId.IsZero)
            {
                this.Stream.WriteLong(this.AccountId);
            }
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            LeaderboardPlayers Leaderboard = Leaderboards.GlobalPlayers;

            if (Leaderboard != null)
            {
                this.Device.NetworkManager.SendMessage(new AvatarRankingListMessage(this.Device, Leaderboard));
            }
            else
            {
                Logging.Error(this.GetType(), "Leaderboard == null at Process().");
            }
        }
    }
}