namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Logic.Mode;

    internal class RefreshAchievementsCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 526;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshAchievementsCommand"/> class.
        /// </summary>
        public RefreshAchievementsCommand()
        {
            // RefreshAchievementsCommand.
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            if (GameMode.Player != null)
            {
                GameMode.AchievementManager.RefreshStatus();
                return 0;
            }

            return 1;
        }
    }
}