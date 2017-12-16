namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class SkinSetData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="SkinSetData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SkinSetData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SkinSetData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Character
        {
            get; set;
        }

        internal string Skin
        {
            get; set;
        }

    }
}