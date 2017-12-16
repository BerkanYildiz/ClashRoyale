namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class DraftDeckData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="DraftDeckData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public DraftDeckData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // DraftDeckData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string RequiredSets
        {
            get; set;
        }

        internal string OptionalSets
        {
            get; set;
        }

    }
}