namespace ClashRoyale.Server.Logic.Commands.Server
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;

    internal class JoinAllianceCommand : ServerCommand
    {
        internal int HighId;
        internal int LowId;

        internal string AllianceName;

        internal bool Created;

        internal AllianceBadgeData AllianceBadgeData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
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
        public JoinAllianceCommand(int HighId, int LowId, string Name, AllianceBadgeData Data, bool Created)
        {
            this.HighId = HighId;
            this.LowId = LowId;
            this.AllianceName = Name;
            this.AllianceBadgeData = Data;
            this.Created = Created;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            this.HighId = Stream.ReadInt();
            this.LowId = Stream.ReadInt();
            this.AllianceName = Stream.ReadString();
            this.AllianceBadgeData = Stream.DecodeData<AllianceBadgeData>();
            this.Created = Stream.ReadBoolean();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteInt(this.HighId);
            Stream.WriteInt(this.LowId);
            Stream.WriteString(this.AllianceName);
            Stream.EncodeData(this.AllianceBadgeData);
            Stream.WriteBoolean(this.Created);

            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            Player Player = GameMode.Player;

            if (Player != null)
            {
                if (this.Created)
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
                Player.SetAllianceName(this.AllianceName);
                Player.SetAllianceRole(this.Created ? 2 : 1);
                Player.SetAllianceBadge(this.AllianceBadgeData);
                
                GameMode.AchievementManager.UpdateAchievementProgress(0, 1);
            }

            GameMode.CommandManager.WaitJoinAllianceTurn = false;

            return 0;
        }
    }
}