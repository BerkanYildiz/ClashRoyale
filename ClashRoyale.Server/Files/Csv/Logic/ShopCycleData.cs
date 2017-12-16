namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class ShopCycleData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ShopCycleData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ShopCycleData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ShopCycleData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int A
        {
            get; set;
        }

        internal int B
        {
            get; set;
        }

        internal int C
        {
            get; set;
        }

        internal string D
        {
            get; set;
        }

    }
}