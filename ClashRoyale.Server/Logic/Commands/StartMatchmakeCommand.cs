namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic.Battle.Manager;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class StartMatchmakeCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 594;
            }
        }

        private bool Is2v2;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartMatchmakeCommand"/> class.
        /// </summary>
        public StartMatchmakeCommand()
        {
            // StartMatchmakeCommand.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.Is2v2 = Stream.ReadBoolean();
            Stream.ReadVInt();
            Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteBoolean(this.Is2v2);
            Stream.WriteVInt(0);
            Stream.WriteVInt(-1);
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
                if (Player != null)
                {
                    if (Player.Arena.TrainingCamp)
                    {
                        return 3;
                    }

                    if (GameMode.Device.Defines.Android == false)
                    {
                        GameMode.Device.NetworkManager.SendMessage(new MatchmakeFailedMessage(GameMode.Device));
                    }

                    BattleManager.AddPlayer(GameMode);

                    return 0;
                }

                return 2;
            }

            return 1;
        }
    }
}