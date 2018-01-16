namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class BuySpellCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
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
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellIndex     = Stream.ReadVInt();
            this.SpellUnknown   = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.SpellIndex);
            Stream.WriteVInt(this.SpellUnknown);
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
                    // TODO : Implement BuySpellCommand()::Execute(GameMode).
                }

                return 2;
            }

            return 1;
        }
    }
}