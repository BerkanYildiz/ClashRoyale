namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class EventOutputData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventOutputData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public EventOutputData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventOutputData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int Id
        {
            get; set;
        }

        internal int Channels
        {
            get; set;
        }

        internal int DurationMillis
        {
            get; set;
        }

    }
}