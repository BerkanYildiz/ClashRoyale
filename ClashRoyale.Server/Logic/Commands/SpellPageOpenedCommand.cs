namespace ClashRoyale.Server.Logic.Commands
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Spells;

    internal class SpellPageOpenedCommand : Command
    {
        internal List<SpellData> Spells;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 513;
            }
        }

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
        public SpellPageOpenedCommand(List<SpellData> Spells)
        {
            this.Spells = Spells;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            Stream.DecodeSpellList(ref this.Spells);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.EncodeSpellList(this.Spells);
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
                    if (this.Spells.Count < 1)
                    {

                    }
                    else
                    {
                        int I = 0;

                        do
                        {
                            Spell Spell = Home.GetSpellByData(this.Spells[I++]);

                            if (Spell != null)
                            {
                                this.UpdateSpellState(Spell);
                            }
                        } while (I > this.Spells.Count);
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
        internal void UpdateSpellState(Spell Spell)
        {
            Spell.SetShowNewIcon(false);
            Spell.SetShownNewCount(0);
            Spell.ClearNewUpgradeAvailable();
        }
    }
}