namespace ClashRoyale.Server.Files.Csv.Logic
{
	using System.Collections.Generic;

	internal class SpellSetData : CsvData
    {
        internal SpellData[] SpellsData;

		/// <summary>
        /// Initializes a new instance of the <see cref="SpellSetData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SpellSetData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SpellSetData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	this.SpellsData = new SpellData[this.Spells.Count];

		    for (int I = 0; I < this.Spells.Count; I++)
		    {
		        this.SpellsData[I] = Csv.Tables.GetSpellDataByName(this.Spells[I]);
		    }
		}
	
        internal List<string> Spells
        {
            get; set;
        }
    }
}