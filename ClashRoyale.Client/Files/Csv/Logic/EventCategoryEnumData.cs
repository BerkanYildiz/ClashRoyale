namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class EventCategoryEnumData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="EventCategoryEnumData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryEnumData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryEnumData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Option
        {
            get; set;
        }

    }
}