namespace ClashRoyale.Server.Logic.Commands.Server
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Logic.Mode;

    internal class LeaveAllianceCommand : ServerCommand
    {
        internal long AllianceId;
        internal bool Kick;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 205;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveAllianceCommand"/> class.
        /// </summary>
        public LeaveAllianceCommand()
        {
            // LeaveAllianceCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveAllianceCommand"/> class.
        /// </summary>
        /// <param name="AllianceId">The alliance identifier.</param>
        /// <param name="Kick">if set to <c>true</c> [kick].</param>
        public LeaveAllianceCommand(long AllianceId, bool Kick) : this()
        {
            this.AllianceId = AllianceId;
            this.Kick       = Kick;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Decode(ByteStream Stream)
        {
            this.AllianceId = Stream.ReadLong();
            this.Kick       = Stream.ReadBoolean();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteLong(this.AllianceId);
            Stream.WriteBoolean(this.Kick);

            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            Home Home       = GameMode.Home;
            Player Player   = GameMode.Player;

            if (Home != null)
            {
                return 1;
            }

            if (Player == null)
            {
                return 2;
            }

            if (Player.IsInAlliance)
            {
                if (Player.AllianceId == this.AllianceId)
                {
                    Player.SetAllianceId(0, 0);
                    Player.SetAllianceRole(0);
                    Player.SetAllianceBadge(null);
                    Player.SetAllianceName(string.Empty);
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                return 3;
            }

            Home.StartDonationCooldown(Globals.LeaveAllianceDonationCooldown);

            GameMode.CommandManager.WaitLeaveAllianceTurn = false;

            return 0;
        }
    }
}