namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class TutorialChestOrderData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="TutorialChestOrderData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TutorialChestOrderData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TutorialChestOrderData.
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

        internal string Npc
        {
            get; set;
        }

        internal string PvETutorial
        {
            get; set;
        }

    }
}