namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;

    public class ShopOpenedCommand : Command
    {
        public int Page;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
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
        public override byte Execute(GameMode GameMode)
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