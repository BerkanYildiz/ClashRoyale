namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;

    internal class BuySpellCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 544;
            }
        }

        private int SpellIndex;
        private int SpellUnknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuySpellCommand"/> class.
        /// </summary>
        public BuySpellCommand()
        {
            // BuySpellCommand.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellIndex = Stream.ReadVInt();
            this.SpellUnknown = Stream.ReadVInt();
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            Home Home = GameMode.Home;

            if (Home != null)
            {
                Player Player = GameMode.Player;

                if (Player != null)
                {
                    // TODO : Implement BuySpellCommand()::Execute(GameMode).
                }

                return 2;
            }

            return 1;
        }
    }
}