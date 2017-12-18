namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class NewsData : CsvData
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
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int Id
        {
            get; set;
        }

        internal bool Enabled
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

        internal string InfoTid
        {
            get; set;
        }

        internal string ItemSwf
        {
            get; set;
        }

        internal string ItemExportName
        {
            get; set;
        }

        internal string IconSwf
        {
            get; set;
        }

        internal string IconExportName
        {
            get; set;
        }

        internal string ImageSwf
        {
            get; set;
        }

        internal string ImageExportName
        {
            get; set;
        }

        internal string ButtonUrl
        {
            get; set;
        }

        internal string ButtonTid
        {
            get; set;
        }

    }
}