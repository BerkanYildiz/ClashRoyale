namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class HelpshiftData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpshiftData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public HelpshiftData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // HelpshiftData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string HelpshiftId
        {
            get; set;
        }

    }
}