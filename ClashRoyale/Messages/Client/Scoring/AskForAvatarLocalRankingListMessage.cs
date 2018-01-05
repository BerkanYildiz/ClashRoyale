namespace ClashRoyale.Messages.Client.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages.Server.Scoring;

    public class AskForAvatarLocalRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14404;
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

        internal LogicLong AccountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarLocalRankingListMessage"/> class.
        /// </summary>
        public AskForAvatarLocalRankingListMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForAvatarLocalRankingListMessage.
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
            LeaderboardPlayers Leaderboard = Leaderboards.GetRegionalPlayers(this.Device.Defines.Region);

            if (Leaderboard != null)
            {
                this.Device.NetworkManager.SendMessage(new AvatarLocaleRankingListMessage(this.Device, Leaderboard));
            }
            else
            {
                Logging.Error(this.GetType(), "Leaderboard == null at Process() with Region == '" + this.Device.Defines.Region + "'.");
            }
        }
    }
}