namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class GlobalData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="GlobalData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public GlobalData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // GlobalData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int NumberValue
        {
            get; set;
        }

        internal bool BooleanValue
        {
            get; set;
        }

        internal string TextValue
        {
            get; set;
        }

        internal string[] StringArray
        {
            get; set;
        }

        internal int[] NumberArray
        {
            get; set;
        }

    }
}