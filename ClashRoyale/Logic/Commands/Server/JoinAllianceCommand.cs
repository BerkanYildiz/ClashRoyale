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

        public string AllianceName;

        public bool Created;

        public AllianceBadgeData AllianceBadgeData;

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
        public override void Decode(ByteStream Stream)
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
        public override void Encode(ChecksumEncoder Stream)
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
        public override byte Execute(GameMode GameMode)
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