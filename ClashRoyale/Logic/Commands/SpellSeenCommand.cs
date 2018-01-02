namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class SpellSeenCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 517;
            }
        }

        private int Unknown;
        private SpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellSeenCommand"/> class.
        /// </summary>
        public SpellSeenCommand()
        {
            // SpellSeenCommand.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.Unknown = Stream.ReadVInt();
            this.Spell = Stream.DecodeData<SpellData>();
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
                    var Card = Home.GetSpellByData(this.Spell);

                    if (Card == null)
                    {
                        return 3;
                    }

                    Card.SetShowNewIcon(false);

                    return 0;
                }

                return 2;
            }

            return 1;
        }
    }
}