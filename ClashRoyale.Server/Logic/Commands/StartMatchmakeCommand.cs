namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Logic.Manager;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Network.Packets.Server.Matchmaking;

    internal class StartMatchmakeCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 525;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartMatchmakeCommand"/> class.
        /// </summary>
        public StartMatchmakeCommand()
        {
            // StartMatchmakeCommand.
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