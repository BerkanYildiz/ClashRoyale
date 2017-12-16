namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class ResourcePackData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ResourcePackData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ResourcePackData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ResourcePackData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Tid
        {
            get; set;
        }

        internal string Resource
        {
            get; set;
        }

        internal int Amount
        {
            get; set;
        }

        internal string IconFile
        {
            get; set;
        }

    }
}