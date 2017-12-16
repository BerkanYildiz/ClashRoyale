namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class ChestOrderData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ChestOrderData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ChestOrderData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ChestOrderData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Chest
        {
            get; set;
        }

        internal int QuestThreshold
        {
            get; set;
        }

        internal string ArenaThreshold
        {
            get; set;
        }

        internal bool OneTime
        {
            get; set;
        }

    }
}