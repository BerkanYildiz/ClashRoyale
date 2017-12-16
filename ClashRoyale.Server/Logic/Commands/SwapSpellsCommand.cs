namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Spells;

    internal class SwapSpellsCommand : Command
    {
        internal int DeckOffset = -1;
        internal int Deck2Offset = -1;
        internal int SpellOffset = -1;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 500;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapSpellsCommand"/> class.
        /// </summary>
        public SwapSpellsCommand()
        {
            // SwapSpellsCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapSpellsCommand"/> class.
        /// </summary>
        public SwapSpellsCommand(int SpellOffset, int DeckOffset, int Deck2Offset)
        {
            this.SpellOffset = SpellOffset;
            this.DeckOffset = DeckOffset;
            this.Deck2Offset = Deck2Offset;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellOffset = Stream.ReadVInt();
            this.DeckOffset = Stream.ReadVInt();
            this.Deck2Offset = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.SpellOffset);
            Stream.WriteVInt(this.DeckOffset);
            Stream.WriteVInt(this.Deck2Offset);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            Home Home = GameMode.Home;

            if (this.SpellOffset == -1)
            {
                if (this.DeckOffset > -1 && this.DeckOffset < 8)
                {
                    if (this.Deck2Offset > -1 && this.Deck2Offset < 8)
                    {
                        if (Home.SpellDeck[this.DeckOffset] != null)
                        {
                            if (Home.SpellDeck[this.Deck2Offset] != null)
                            {
                                if (this.DeckOffset != this.Deck2Offset)
                                {
                                    Home.SpellDeck.SwapSpells(this.DeckOffset, this.Deck2Offset);
                                    Home.SaveCurrentDeckTo(Home.SelectedDeck);
                                }

                                return 0;
                            }

                            return 4;
                        }

                        return 3;
                    }

                    return 2;
                }

                return 1;
            }
            else
            {
                if (this.SpellOffset > -1 && this.SpellOffset < Home.SpellCollection.Count)
                {
                    if (this.DeckOffset > -1 && this.DeckOffset < 8)
                    {
                        Spell Insert = Home.SpellCollection[this.SpellOffset];
                        Spell SpellInDeck = Home.SpellDeck[this.DeckOffset];

                        if (Insert != null)
                        {
                            if (SpellInDeck != null)
                            {
                                if (Home.SpellDeck.CanBeInserted(this.DeckOffset, Insert))
                                {
                                    Home.SpellDeck.SetSpell(this.DeckOffset, Insert);
                                    Home.SpellCollection.SetSpell(this.SpellOffset, SpellInDeck);

                                    Home.SaveCurrentDeckTo(Home.SelectedDeck);

                                    return 0;
                                }

                                return 5;
                            }

                            return 4;
                        }

                        return 3;
                    }

                    return 2;
                }

                return 1;
            }
        }
    }
}