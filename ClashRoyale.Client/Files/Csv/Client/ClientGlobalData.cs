namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class ClientGlobalData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGlobalData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public ClientGlobalData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ClientGlobalData.
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