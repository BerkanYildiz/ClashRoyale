namespace ClashRoyale.Server.Logic.Spells
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Math = ClashRoyale.Server.Logic.Math;

    [JsonConverter(typeof(SpellCollectionConverter))]
    internal class SpellCollection
    {
        private List<Spell> Collections;

        internal Spell this[int Index]
        {
            get
            {
                if (this.Collections.Count > Index)
                {
                    return this.Collections[Index];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the number of spells in collection.
        /// </summary>
        internal int Count
        {
            get
            {
                return this.Collections.Count;
            }
        }

        /// <summary>
        /// Gets the latest create time.
        /// </summary>
        internal int LatestCreateTime
        {
            get
            {
                int Max = 0;

                this.Collections.ForEach(Spell =>
                {
                    Max = Math.Max(Spell.CreateTime, Max);
                });

                return Max;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCollection"/> class.
        /// </summary>
        internal SpellCollection()
        {
            this.Collections = new List<Spell>(40);
        }

        /// <summary>
        /// Adds the specified spell in collection.
        /// </summary>
        internal void AddSpell(Spell Spell)
        {
            if (!this.CanAddSpell(Spell))
            {
                Logging.Error(this.GetType(), "AddSpell() - Trying to add spell that already exists in collection, data:" + Spell.Data + ".");
            }

            this.Collections.Add(Spell);
        }

        /// <summary>
        /// Returns if can add the specified spell in collection.
        /// </summary>
        internal bool CanAddSpell(Spell Spell)
        {
            return !this.Collections.Contains(Spell);
        }

        /// <summary>
        /// Gets a spell by data.
        /// </summary>
        internal Spell GetSpellByData(SpellData Data)
        {
            return this.Collections.Find(Spell => Spell.Data == Data);
        }

        /// <summary>
        /// Gets the spell index in the collection by data.
        /// </summary>
        internal int GetSpellIdxByData(SpellData Data)
        {
            return this.Collections.FindIndex(Spell => Spell.Data == Data);
        }

        /// <summary>
        /// Removes the specified spell.
        /// </summary>
        internal void RemoveSpell(Spell Spell)
        {
            this.Collections.Remove(Spell);
        }

        /// <summary>
        /// Removes the element located at specified index..
        /// </summary>
        internal void RemoveSpell(int Idx)
        {
            this.Collections.RemoveAt(Idx);
        }

        /// <summary>
        /// Sets the spell at specified index.
        /// </summary>
        internal void SetSpell(int Index, Spell Spell)
        {
            this.Collections[Index] = Spell;
        }

        /// <summary>
        /// Swap two spells in collection.
        /// </summary>
        internal Spell SwapSpells(Spell Spell, int Index)
        {
            Spell Swap = this.Collections[Index];
            this.Collections[Index] = Spell;
            return Swap;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream Reader)
        {
            for (int I = Reader.ReadVInt(); I > 0; I--)
            {
                Spell Spell = new Spell(null);
                Spell.Decode(Reader);
                this.Collections.Add(Spell);
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(this.Collections.Count);

            this.Collections.ForEach(Spell =>
            {
                Spell.Encode(Stream);
            });
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal void Load(JArray Array)
        {
            for (int I = 0; I < Array.Count; I++)
            {
                Spell Spell = new Spell(null);
                Spell.Load(Array[I]);
                this.Collections.Add(Spell);
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JArray Save()
        {
            JArray Spells = new JArray();

            this.Collections.ForEach(Spell =>
            {
                Spells.Add(Spell.Save());
            });

            return Spells;
        }
    }

    internal class SpellCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            SpellCollection Collection = (SpellCollection) Value;

            if (Collection != null)
            {
                Collection.Save().WriteTo(Writer);
            }
            else
                Writer.WriteNull();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            SpellCollection Deck = (SpellCollection) ExistingValue;

            if (Deck == null)
            {
                Deck = new SpellCollection();
            }

            Deck.Load(JArray.Load(Reader));

            return Deck;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(SpellCollection);
        }
    }
}