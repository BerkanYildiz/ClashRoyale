namespace ClashRoyale.Logic.Home.Spells
{
    using System.Collections.Generic;

    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Converters;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Math = ClashRoyale.Maths.Math;

    [JsonConverter(typeof(SpellCollectionConverter))]
    public class SpellCollection
    {
        private readonly List<Spell> Spells;

        public Spell this[int Index]
        {
            get
            {
                if (this.Spells.Count > Index)
                {
                    return this.Spells[Index];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the number of spells in collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.Spells.Count;
            }
        }

        /// <summary>
        /// Gets the latest create time.
        /// </summary>
        public int LatestCreateTime
        {
            get
            {
                int Max = 0;

                this.Spells.ForEach(Spell =>
                {
                    Max = Math.Max(Spell.CreateTime, Max);
                });

                return Max;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCollection"/> class.
        /// </summary>
        public SpellCollection()
        {
            this.Spells = new List<Spell>(40);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCollection"/> class.
        /// </summary>
        /// <param name="Capacity">The capacity.</param>
        public SpellCollection(int Capacity)
        {
            this.Spells = new List<Spell>(Capacity);
        }

        /// <summary>
        /// Adds the specified spell in collection.
        /// </summary>
        public void AddSpell(Spell Spell)
        {
            if (!this.CanAddSpell(Spell))
            {
                Logging.Error(this.GetType(), "AddSpell() - Trying to add spell that already exists in collection, data:" + Spell.Data + ".");
            }

            this.Spells.Add(Spell);
        }

        /// <summary>
        /// Returns if can add the specified spell in collection.
        /// </summary>
        public bool CanAddSpell(Spell Spell)
        {
            return !this.Spells.Contains(Spell);
        }

        /// <summary>
        /// Gets a spell by data.
        /// </summary>
        public Spell GetSpellByData(SpellData Data)
        {
            return this.Spells.Find(Spell => Spell.Data == Data);
        }

        /// <summary>
        /// Gets the spell index in the collection by data.
        /// </summary>
        public int GetSpellIdxByData(SpellData Data)
        {
            return this.Spells.FindIndex(Spell => Spell.Data == Data);
        }

        /// <summary>
        /// Gets the spells.
        /// </summary>
        public List<Spell> GetSpells()
        {
            return this.Spells;
        }

        /// <summary>
        /// Removes the specified spell.
        /// </summary>
        public void RemoveSpell(Spell Spell)
        {
            this.Spells.Remove(Spell);
        }

        /// <summary>
        /// Removes the element located at specified index..
        /// </summary>
        public void RemoveSpell(int Idx)
        {
            this.Spells.RemoveAt(Idx);
        }

        /// <summary>
        /// Sets the spell at specified index.
        /// </summary>
        public void SetSpell(int Index, Spell Spell)
        {
            this.Spells[Index] = Spell;
        }

        /// <summary>
        /// Swap two spells in collection.
        /// </summary>
        public Spell SwapSpells(Spell Spell, int Index)
        {
            Spell Swap = this.Spells[Index];
            this.Spells[Index] = Spell;
            return Swap;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public void Decode(ByteStream Reader)
        {
            for (int I = Reader.ReadVInt(); I > 0; I--)
            {
                Spell Spell = new Spell(null);
                Spell.Decode(Reader);
                this.Spells.Add(Spell);
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(this.Spells.Count);

            this.Spells.ForEach(Spell =>
            {
                Spell.Encode(Stream);
            });
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public void Load(JArray Array)
        {
            for (int I = 0; I < Array.Count; I++)
            {
                Spell Spell = new Spell(null);
                Spell.Load(Array[I]);
                this.Spells.Add(Spell);
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public JArray Save()
        {
            JArray Spells = new JArray();

            this.Spells.ForEach(Spell =>
            {
                Spells.Add(Spell.Save());
            });

            return Spells;
        }
    }
}