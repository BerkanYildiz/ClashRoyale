namespace ClashRoyale.Messages.Client.Scoring
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages.Server.Scoring;

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

        private LogicLong AllianceId;
        private bool IsLocal;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAllianceRankingListMessage"/> class.
        /// </summary>
        public AskForAllianceRankingListMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
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
                this.AllianceId = this.Stream.ReadLong();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(this.IsLocal);
            this.Stream.WriteBoolean(!this.AllianceId.IsZero);

            if (!this.AllianceId.IsZero)
            {
                this.Stream.WriteLong(this.AllianceId);
            }
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            LeaderboardClans Leaderboard = Leaderboards.GlobalClans;

            if (Leaderboard != null)
            {
                if (!this.IsLocal)
                {
                    this.Device.NetworkManager.SendMessage(new AllianceRankingListMessage(this.Device, Leaderboard));
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new AllianceLocaleRankingListMessage(this.Device, Leaderboard));
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Leaderboard == null at Process() with Region == '" + this.Device.Defines.Region + "'.");
            }
        }
    }
}