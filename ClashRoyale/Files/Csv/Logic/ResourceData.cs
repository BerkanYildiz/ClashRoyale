namespace ClashRoyale.Files.Csv.Logic
{
    public class ResourceData : CsvData
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
		public override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        public string Tid
        {
            get; set;
        }

        public string IconSwf
        {
            get; set;
        }

        public bool UsedInBattle
        {
            get; set;
        }

        public string CollectEffect
        {
            get; set;
        }

        public string IconExportName
        {
            get; set;
        }

        public bool PremiumCurrency
        {
            get; set;
        }

        public string CapFullTid
        {
            get; set;
        }

        public int TextRed
        {
            get; set;
        }

        public int TextGreen
        {
            get; set;
        }

        public int TextBlue
        {
            get; set;
        }

        public int Cap
        {
            get; set;
        }

        public string IconFile
        {
            get; set;
        }

        public string ShopIcon
        {
            get; set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a cap.
        /// </summary>
        public bool HasCap
        {
            get
            {
                return this.Cap > 0;
            }
        }
    }
}