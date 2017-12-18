namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class RegionData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="RegionData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public RegionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // RegionData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Tid
        {
            get; set;
        }

        internal string DisplayName
        {
            get; set;
        }

        internal bool IsCountry
        {
            get; set;
        }

        internal bool RegionPopup
        {
            get; set;
        }

    }
}