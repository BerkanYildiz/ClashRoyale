namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class BillingPackageData : CsvData
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
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Tid
        {
            get; set;
        }

        internal bool Disabled
        {
            get; set;
        }

        internal bool ExistsApple
        {
            get; set;
        }

        internal bool ExistsAndroid
        {
            get; set;
        }

        internal bool ExistsKunlun
        {
            get; set;
        }

        internal bool ExistsJupiter
        {
            get; set;
        }

        internal int Diamonds
        {
            get; set;
        }

        internal int Usd
        {
            get; set;
        }

        internal int Rmb
        {
            get; set;
        }

        internal int Order
        {
            get; set;
        }

        internal string IconFile
        {
            get; set;
        }

        internal string JupiterId
        {
            get; set;
        }

        internal string StarterPackName
        {
            get; set;
        }

        internal bool IsRedPackage
        {
            get; set;
        }

        internal string RumblePackName
        {
            get; set;
        }

        internal string ChronosOfferName
        {
            get; set;
        }

        internal int RedeemMax
        {
            get; set;
        }

        internal int CampaignId
        {
            get; set;
        }

    }
}