namespace ClashRoyale.Files.Csv.Logic
{
    using System.Collections.Generic;

    public class SpellSetData : CsvData
    {
        public SpellData[] SpellsData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpellSetData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SpellSetData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SpellSetData.
        }

        public List<string> Spells { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            this.SpellsData = new SpellData[this.Spells.Count];

            for (int I = 0; I < this.Spells.Count; I++)
            {
                this.SpellsData[I] = CsvFiles.GetSpellDataByName(this.Spells[I]);
            }
        }
    }
}