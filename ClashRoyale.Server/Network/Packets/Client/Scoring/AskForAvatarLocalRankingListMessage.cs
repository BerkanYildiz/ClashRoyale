namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Scoring;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForAvatarLocalRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14404;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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

            if (!this.AccountId.IsZero)
            {
                this.Stream.WriteLong(this.AccountId);
            }
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
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