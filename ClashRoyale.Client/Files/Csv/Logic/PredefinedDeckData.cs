namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class PredefinedDeckData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="PredefinedDeckData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public PredefinedDeckData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // PredefinedDeckData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Spells
        {
            get; set;
        }

        internal int SpellLevel
        {
            get; set;
        }

        internal string RandomSpellSets
        {
            get; set;
        }

        internal string Description
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

    }
}