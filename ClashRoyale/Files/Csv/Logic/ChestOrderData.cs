namespace ClashRoyale.Files.Csv.Logic
{
    public class ChestOrderData : CsvData
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
		public override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        public string Chest
        {
            get; set;
        }

        public int QuestThreshold
        {
            get; set;
        }

        public string ArenaThreshold
        {
            get; set;
        }

        public bool OneTime
        {
            get; set;
        }

    }
}