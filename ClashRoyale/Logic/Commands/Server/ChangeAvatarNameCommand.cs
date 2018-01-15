namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class ChangeAvatarNameCommand : ServerCommand
    {
        public string Name;
        public bool NameSetByUser;
        public int NameChangeState;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 278;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameCommand"/> class.
        /// </summary>
        public ChangeAvatarNameCommand()
        {
            // ChangeAvatarNameCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameCommand"/> class.
        /// </summary>
        public ChangeAvatarNameCommand(string Name, bool NameSetByUser, int NameChangeState)
        {
            this.Name               = Name;
            this.NameSetByUser      = NameSetByUser;
            this.NameChangeState    = NameChangeState;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            this.Name               = Stream.ReadString();
            this.NameChangeState    = Stream.ReadInt();
            this.NameSetByUser      = Stream.ReadBoolean();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteString(this.Name);
            Stream.WriteInt(this.NameChangeState);
            Stream.WriteBoolean(this.NameSetByUser);

            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Player Player = GameMode.Player;
            byte FailCode = 1;

            if (Player != null)
            {
                Player.SetName(this.Name);
                Player.SetNameSetByUser(this.NameSetByUser);
                Player.SetNameChangeState(this.NameChangeState);

                FailCode = 0;
            }

            GameMode.CommandManager.WaitChangeAvatarNameTurn = false;

            return FailCode;
        }
    }
}