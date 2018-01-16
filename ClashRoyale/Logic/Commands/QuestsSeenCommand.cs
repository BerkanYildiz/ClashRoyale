namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Logic.Mode;

    public class QuestsSeenCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 522;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestsSeenCommand"/> class.
        /// </summary>
        public QuestsSeenCommand()
        {
            // QuestsSeenCommand.
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            return 0;
        }
    }
}