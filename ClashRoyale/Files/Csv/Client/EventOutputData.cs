namespace ClashRoyale.Files.Csv.Client
{
    public class EventOutputData : CsvData
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
		public override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        public int Id
        {
            get; set;
        }

        public int Channels
        {
            get; set;
        }

        public int DurationMillis
        {
            get; set;
        }

    }
}