namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class ResourceData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ResourceData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ResourceData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ResourceData.
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

        internal string IconSwf
        {
            get; set;
        }

        internal bool UsedInBattle
        {
            get; set;
        }

        internal string CollectEffect
        {
            get; set;
        }

        internal string IconExportName
        {
            get; set;
        }

        internal bool PremiumCurrency
        {
            get; set;
        }

        internal string CapFullTid
        {
            get; set;
        }

        internal int TextRed
        {
            get; set;
        }

        internal int TextGreen
        {
            get; set;
        }

        internal int TextBlue
        {
            get; set;
        }

        internal int Cap
        {
            get; set;
        }

        internal string IconFile
        {
            get; set;
        }

        internal string ShopIcon
        {
            get; set;
        }

    }
}