namespace ClashRoyale.Server.Logic.Commands.Server
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;

    internal class ChangeAllianceRoleCommand : ServerCommand
    {
        private long AllianceId;
        private int NewRole;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 207;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAllianceRoleCommand"/> class.
        /// </summary>
        public ChangeAllianceRoleCommand()
        {
            // ChangeAllianceRoleCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAllianceRoleCommand"/> class.
        /// </summary>
        public ChangeAllianceRoleCommand(long AllianceId, int NewRole)
        {
            this.AllianceId = AllianceId;
            this.NewRole = NewRole;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            this.AllianceId = Stream.ReadLong();
            this.NewRole = Stream.ReadVInt();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteLong(this.AllianceId);
            Stream.WriteVInt(this.NewRole);

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
                if (Player.AllianceId == this.AllianceId)
                {
                    Player.SetAllianceRole(this.NewRole);
                }

                return 0;
            }

            return 1;
        }
    }
}