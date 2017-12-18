namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class TauntData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="TauntData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TauntData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TauntData.
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

        internal bool TauntMenu
        {
            get; set;
        }

        internal string FileName
        {
            get; set;
        }

        internal string ExportName
        {
            get; set;
        }

        internal string IconExportName
        {
            get; set;
        }

        internal string BtnExportName
        {
            get; set;
        }

        internal string Sound
        {
            get; set;
        }

    }
}