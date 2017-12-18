namespace ClashRoyale.Files.Csv.Client
{
    public class BillingPackageData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BillingPackageData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public BillingPackageData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // BillingPackageData.
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

        public bool Disabled
        {
            get; set;
        }

        public bool ExistsApple
        {
            get; set;
        }

        public bool ExistsAndroid
        {
            get; set;
        }

        public bool ExistsKunlun
        {
            get; set;
        }

        public bool ExistsJupiter
        {
            get; set;
        }

        public int Diamonds
        {
            get; set;
        }

        public int Usd
        {
            get; set;
        }

        public int Rmb
        {
            get; set;
        }

        public int Order
        {
            get; set;
        }

        public string IconFile
        {
            get; set;
        }

        public string JupiterId
        {
            get; set;
        }

        public string StarterPackName
        {
            get; set;
        }

        public bool IsRedPackage
        {
            get; set;
        }

        public string RumblePackName
        {
            get; set;
        }

        public string ChronosOfferName
        {
            get; set;
        }

        public int RedeemMax
        {
            get; set;
        }

        public int CampaignId
        {
            get; set;
        }

    }
}