namespace ClashRoyale.Files.Csv.Client
{
    public class NewsData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="NewsData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public NewsData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // NewData.
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

        public bool Enabled
        {
            get; set;
        }

        public string Tid
        {
            get; set;
        }

        public string InfoTid
        {
            get; set;
        }

        public string ItemSwf
        {
            get; set;
        }

        public string ItemExportName
        {
            get; set;
        }

        public string IconSwf
        {
            get; set;
        }

        public string IconExportName
        {
            get; set;
        }

        public string ImageSwf
        {
            get; set;
        }

        public string ImageExportName
        {
            get; set;
        }

        public string ButtonUrl
        {
            get; set;
        }

        public string ButtonTid
        {
            get; set;
        }

    }
}