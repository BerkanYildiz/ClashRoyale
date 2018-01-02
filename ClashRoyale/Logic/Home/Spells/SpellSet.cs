namespace ClashRoyale.Logic.Home.Spells
{
    using System.Collections.Generic;

    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Enums;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;

    public class SpellSet
    {
        public readonly List<SpellData>[] Spells;

        /// <summary>
        /// Gets the number of spells.
        /// </summary>
        public int Count
        {
            get
            {
                int Count = 0;

                for (int I = 0; I < this.Spells.Length; I++)
                {
                    Count += this.Spells[I].Count;
                }

                return Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellSet"/> class.
        /// </summary>
        public SpellSet(ArenaData ArenaData, SpellSetData SetData)
        {
            this.Spells = new List<SpellData>[CsvFiles.Get(Gamefile.Rarities).Datas.Count];

            for (int I = 0; I < CsvFiles.Get(Gamefile.Rarities).Datas.Count; I++)
            {
                this.Spells[I] = new List<SpellData>(32);
            }

            if (SetData != null)
            {
                for (int I = 0; I < SetData.SpellsData.Length; I++)
                {
                    this.AddSpell(SetData.SpellsData[I]);
                }
            }
            else
            {
                CsvFiles.Spells.ForEach(SpellData =>
                {
                    if (SpellData.IsUnlockedInArena(ArenaData))
                    {
                        this.AddSpell(SpellData);
                    }
                });
            }
        }

        /// <summary>
        /// Adds the spell to the collection.
        /// </summary>
        public void AddSpell(SpellData Data)
        {
            if (!Data.NotInUse)
            {
                this.Spells[Data.RarityData.Instance].Add(Data);
            }
        }
        
        /// <summary>
        /// Gets a random spell.
        /// </summary>
        public SpellData GetRandomSpell(RarityData Data)
        {
            int Count = this.Spells[Data.Instance].Count;

            if (Count > 0)
            {
                return this.Spells[Data.Instance][XorShift.Next(Count)];
            }

            Logging.Warning(this.GetType(), "GetRandomSpell() - No spell found for rarity: " + Data.Name);

            return null;
        }
    }
}