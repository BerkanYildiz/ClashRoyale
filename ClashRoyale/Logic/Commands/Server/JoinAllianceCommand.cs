namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class JoinAllianceCommand : ServerCommand
    {
        public int HighId;
        public int LowId;

        public string Name;

        public bool Creation;

        public AllianceBadgeData BadgeData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 206;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceCommand"/> class.
        /// </summary>
        public JoinAllianceCommand()
        {
            // JoinAllianceCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceCommand"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="BadgeData">The badge data.</param>
        /// <param name="Creation">if set to <c>true</c> [creation].</param>
        public JoinAllianceCommand(int HighId, int LowId, string Name, AllianceBadgeData BadgeData, bool Creation)
        {
            this.HighId     = HighId;
            this.LowId      = LowId;
            this.Name       = Name;
            this.BadgeData  = BadgeData;
            this.Creation   = Creation;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            this.HighId     = Stream.ReadInt();
            this.LowId      = Stream.ReadInt();
            this.Name       = Stream.ReadString();
            this.BadgeData  = Stream.DecodeData<AllianceBadgeData>();
            this.Creation   = Stream.ReadBoolean();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteInt(this.HighId);
            Stream.WriteInt(this.LowId);
            Stream.WriteString(this.Name);
            Stream.EncodeData(this.BadgeData);
            Stream.WriteBoolean(this.Creation);

            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Player Player = GameMode.Player;

            if (Player != null)
            {
                if (this.Creation)
                {
                    if (Player.Gold >= Globals.AllianceCreateCost)
                    {
                        Player.UseGold(Globals.AllianceCreateCost);
                    }
                    else
                    {
                        Player.SetFreeGold(0);
                    }
                }

                Player.SetAllianceId(this.HighId, this.LowId);
                Player.SetAllianceName(this.Name);
                Player.SetAllianceRole(this.Creation ? 2 : 1);
                Player.SetAllianceBadge(this.BadgeData);
                
                GameMode.AchievementManager.UpdateAchievementProgress(0, 1);
            }

            GameMode.CommandManager.WaitJoinAllianceTurn = false;

            return 0;
        }
    }
}