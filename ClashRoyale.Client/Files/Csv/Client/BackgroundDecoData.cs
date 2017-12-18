namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class BackgroundDecoData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundDecoData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public BackgroundDecoData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // BackgroundDecoData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string FileName
        {
            get; set;
        }

        internal string ExportName
        {
            get; set;
        }

        internal string Layer
        {
            get; set;
        }
    }
}