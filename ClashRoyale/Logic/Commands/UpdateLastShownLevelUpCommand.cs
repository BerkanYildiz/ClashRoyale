namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class UpdateLastShownLevelUpCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 576;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLastShownLevelUpCommand"/> class.
        /// </summary>
        public UpdateLastShownLevelUpCommand()
        {
            // UpdateLastShownLevelUpCommand.
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Home Home = GameMode.Home;

            if (Home != null)
            {
                Player Player = GameMode.Player;

                if (Player != null)
                {
                    if (Player.ExpLevel > Home.LastLevelUpPopup)
                    {
                        Home.SetLastShownLevelUp(Player.ExpLevel);
                    }

                    return 0;
                }

                return 2;
            }

            return 1;
        }
    }
}