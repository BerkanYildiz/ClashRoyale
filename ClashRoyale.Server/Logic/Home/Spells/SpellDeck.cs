namespace ClashRoyale.Server.Logic.Home.Spells
{
    using System;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(SpellDeckConverter))]
    internal class SpellDeck
    {
        private Spell[] Spells;

        /// <summary>
        /// Gets if this instance has a empty slot.
        /// </summary>
        internal bool Empty
        {
            get
            {
                for (int I = 0; I < 8; I++)
                {
                    if (this.Spells[I] == null)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets if this instance is full
        /// </summary>
        internal bool Full
        {
            get
            {
                for (int I = 0; I < 8; I++)
                {
                    if (this.Spells[I] == null)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the spell count in collection.
        /// </summary>
        internal int SpellCount
        {
            get
            {
                int Count = 0;

                for (int I = 0; I < 8; I++)
                {
                    if (this.Spells[I] != null)
                        ++Count;
                }

                return Count;
            }
        }

        /// <summary>
        /// Gets the spell at specified index.
        /// </summary>
        internal Spell this[int Idx]
        {
            get
            {
                if (Idx >= 8)
                {
                    return null;
                }

                return this.Spells[Idx];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellDeck"/> class.
        /// </summary>
        public SpellDeck()
        {
            this.Spells = new Spell[8];
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        internal SpellDeck Clone()
        {
            SpellDeck SpellDeck = new SpellDeck();

            for (int I = 0; I < 8; I++)
            {
                if (this.Spells[I] != null)
                {
                    SpellDeck.Spells[I] = this.Spells[I].Clone();
                }
            }

            return SpellDeck;
        }

        /// <summary>
        /// Returns if can be insert the specified spell.
        /// </summary>
        internal bool CanBeInserted(int Index, Spell Spell)
        {
            if (Index < 8)
            {
                Spell Existing = this.Spells[Index];

                if (Existing != null)
                {
                    if (Existing.Data.Equals(Spell.Data) && !Existing.Equals(Spell))
                    {
                        return true;
                    }
                }

                for (int I = 0; I < 8; I++)
                {
                    Existing = this.Spells[I];

                    if (Existing != null)
                    {
                        if (Existing.Equals(Spell))
                        {
                            return false;
                        }

                        if (Existing.Data.Equals(Spell.Data))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if this instance contains the specified spell data.
        /// </summary>
        internal bool ContainsSpellData(SpellData SpellData)
        {
            return this.GetSpellByData(SpellData) != null;
        }

        /// <summary>
        /// Returns if this instance contains the specified spell data.
        /// </summary>
        internal void MoveSpellFromCollection(int DeckIndex, int CollectionIndex, SpellCollection Collection)
        {
            Spell SpellCollection = Collection[DeckIndex];

            if (this.CanBeInserted(DeckIndex, SpellCollection))
            {
                if (this.Spells[DeckIndex] != null)
                {
                    this.Spells[DeckIndex] = Collection.SwapSpells(this.Spells[DeckIndex], CollectionIndex);
                }
                else
                    this.PutSpellInEmptySlot(DeckIndex, SpellCollection);
            }
            else
                Logging.Error(this.GetType(), "CanBeInserted returns false, should check it before trying to move.");
        }

        /// <summary>
        /// Gets the spell id in collection by data.
        /// </summary>
        internal int GetSpellIdxByData(SpellData Data)
        {
            return Array.FindIndex(this.Spells, S => S.Data == Data);
        }

        /// <summary>
        /// Gets a spell by data.
        /// </summary>
        internal Spell GetSpellByData(SpellData Data)
        {
            return Array.Find(this.Spells, S => S.Data == Data);
        }

        /// <summary>
        /// Puts the spell in empty slot.
        /// </summary>
        internal void PutSpellInEmptySlot(int Index, Spell Spell)
        {
            if (Index >= 8)
            {
                Logging.Error(this.GetType(), "PutSpell() - Index is out of bounds " + (Index + 1) + "/" + 8 + ".");
                return;
            }

            if (this.Spells[Index] != null)
            {
                Logging.Error(this.GetType(), "PutSpell() - Trying to overwrite a spell at " + Index + ".");
                return;
            }

            this.Spells[Index] = Spell;
        }

        /// <summary>
        /// Sets the spell at specified index.
        /// </summary>
        internal void SetSpell(int Index, Spell Spell)
        {
            this.Spells[Index] = Spell;
        }

        /// <summary>
        /// Swaps two spells.
        /// </summary>
        internal void SwapSpells(int SpellId1, int SpellId2)
        {
            Spell SwapTemp = this.Spells[SpellId1];
            this.Spells[SpellId1] = this.Spells[SpellId2];
            this.Spells[SpellId2] = SwapTemp;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream Reader)
        {
            for (int I = 0; I < 8; I++)
            {
                if (Reader.ReadBoolean())
                {
                    this.Spells[I] = new Spell(null);
                }
            }

            for (int I = 0; I < 8; I++)
            {
                if (this.Spells[I] != null)
                {
                    this.Spells[I].Decode(Reader);
                }
            }
        }

        /// <summary>
        /// Decodes this instance with attack method.
        /// </summary>
        internal void DecodeAttack(ByteStream Reader)
        {
            for (int I = 0; I < 8; I++)
            {
                if (Reader.ReadBoolean())
                {
                    this.Spells[I] = new Spell(null);
                }
            }

            for (int I = 0; I < 8; I++)
            {
                if (this.Spells[I] != null)
                {
                    this.Spells[I].DecodeAttack(Reader);
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder Stream)
        {
            for (int I = 0; I < 8; I++)
            {
                Stream.WriteBoolean(this.Spells[I] != null);
            }

            for (int I = 0; I < 8; I++)
            {
                if (this.Spells[I] != null)
                {
                    this.Spells[I].Encode(Stream);
                }
            }
        }

        /// <summary>
        /// Encodes this instance with attack method.
        /// </summary>
        internal void EncodeAttack(ChecksumEncoder Stream)
        {
            for (int I = 0; I < 8; I++)
            {
                Stream.WriteBoolean(this.Spells[I] != null);
            }

            for (int I = 0; I < 8; I++)
            {
                if (this.Spells[I] != null)
                {
                    this.Spells[I].EncodeAttack(Stream);
                }
            }
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal void Load(JArray Array)
        {
            for (int I = 0; I < Array.Count; I++)
            {
                this.Spells[I] = new Spell(null);
                this.Spells[I].Load(Array[I]);
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JArray Save()
        {
            JArray Array = new JArray();

            for (int I = 0; I < 8; I++)
            {
                if (this.Spells[I] != null)
                {
                    Array.Add(this.Spells[I].Save());
                }
            }

            return Array;
        }
    }

    internal class SpellDeckConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            SpellDeck Deck = (SpellDeck) Value;

            if (Deck != null)
            {
                Deck.Save().WriteTo(Writer);
            }
            else
                Writer.WriteNull();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            SpellDeck Deck = (SpellDeck) ExistingValue;

            if (Deck == null)
            {
                Deck = new SpellDeck();
            }

            Deck.Load(JArray.Load(Reader));

            return Deck;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(SpellDeck);
        }
    }
}