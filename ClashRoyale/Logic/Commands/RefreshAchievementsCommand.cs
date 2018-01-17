namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Logic.Mode;

    public class RefreshAchievementsCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 518;
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
        public override byte Execute(GameMode GameMode)
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