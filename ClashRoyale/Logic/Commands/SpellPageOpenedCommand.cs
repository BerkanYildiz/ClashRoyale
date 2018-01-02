namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class SpellPageOpenedCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 599;
            }
        }

        private bool Seen;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellPageOpenedCommand"/> class.
        /// </summary>
        public SpellPageOpenedCommand()
        {
            // SpellPageOpenedCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellPageOpenedCommand"/> class.
        /// </summary>
        public SpellPageOpenedCommand(bool Seen)
        {
            this.Seen = Seen;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.Seen = Stream.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.WriteBool(this.Seen);
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
                    if (this.Seen)
                    {
                        var Spells = Home.SpellCollection.GetSpells();

                        foreach (var Spell in Spells)
                        {
                            this.UpdateSpellState(Spell);
                        }
                    }

                    return 0;
                }

                return 2;
            }

            return 1;
        }

        /// <summary>
        /// Updates the state of the specified spell.
        /// </summary>
        public void UpdateSpellState(Spell Spell)
        {
            Spell.SetShowNewIcon(false);
            Spell.SetShownNewCount(0);
            Spell.ClearNewUpgradeAvailable();
        }
    }
}