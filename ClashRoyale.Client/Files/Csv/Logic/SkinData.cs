namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class SkinData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="SkinData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SkinData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SkinData.
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

        internal string ExportNameRed
        {
            get; set;
        }

        internal string TopExportName
        {
            get; set;
        }

        internal string TopExportNameRed
        {
            get; set;
        }

        internal string Character
        {
            get; set;
        }

        internal int ValueGems
        {
            get; set;
        }

        internal string Tid
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

        internal bool IsInUse
        {
            get; set;
        }

        internal string DeathEffect
        {
            get; set;
        }

        internal string DeathEffect2
        {
            get; set;
        }

        internal bool EventSkin
        {
            get; set;
        }

    }
}