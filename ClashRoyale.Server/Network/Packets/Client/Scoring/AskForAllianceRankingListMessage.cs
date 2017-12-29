namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Scoring;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForAllianceRankingListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14171;
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
        internal override void Decode()
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
        internal override void Encode()
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
        internal override void Process()
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