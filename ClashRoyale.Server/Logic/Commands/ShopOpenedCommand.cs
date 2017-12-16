namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Logic.Mode;

    internal class ShopOpenedCommand : Command
    {
        internal int Page;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 520;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopOpenedCommand"/> class.
        /// </summary>
        public ShopOpenedCommand()
        {
            // SpellOpenedCommand.
        }
        
        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            Home Home = GameMode.Home;

            if (Home != null)
            {
                Home.SetShopWeekdayIndexSeen(Home.ShopDay);

                return 0;
            }

            return 1;
        }
    }
}