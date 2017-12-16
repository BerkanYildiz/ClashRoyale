namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class EventCategoryData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="EventCategoryData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string CsvFiles
        {
            get; set;
        }

        internal string CsvRows
        {
            get; set;
        }

        internal string CustomNames
        {
            get; set;
        }

    }
}