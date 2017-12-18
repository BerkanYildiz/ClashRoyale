namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class ContentTestData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ContentTestData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ContentTestData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ContentTestData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string SourceData
        {
            get; set;
        }

        internal string TargetData
        {
            get; set;
        }

        internal string Stat1
        {
            get; set;
        }

        internal string Operator
        {
            get; set;
        }

        internal string Stat2
        {
            get; set;
        }

        internal int Result
        {
            get; set;
        }

        internal bool Enabled
        {
            get; set;
        }

    }
}