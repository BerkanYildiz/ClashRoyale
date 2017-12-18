namespace ClashRoyale.Files.Csv.Client
{
    public class LocaleData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LocaleData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public LocaleData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // LocaleData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		public override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        public bool Enabled
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public int SortOrder
        {
            get; set;
        }

        public bool HasEvenSpaceCharacters
        {
            get; set;
        }

        public string UsedSystemFont
        {
            get; set;
        }

        public string HelpshiftSdkLanguage
        {
            get; set;
        }

        public string HelpshiftSdkLanguageAndroid
        {
            get; set;
        }

        public string HelpshiftLanguageTag
        {
            get; set;
        }

        public string TermsAndServiceUrl
        {
            get; set;
        }

        public string TournamentTermsUrl
        {
            get; set;
        }

        public string ParentsGuideUrl
        {
            get; set;
        }

        public string PrivacyPolicyUrl
        {
            get; set;
        }

        public bool TestLanguage
        {
            get; set;
        }

        public string TestExcludes
        {
            get; set;
        }

        public string RegionListFile
        {
            get; set;
        }

        public bool MaintenanceRoyalBox
        {
            get; set;
        }

        public string RoyalBoxUrl
        {
            get; set;
        }

        public string RoyalBoxStageUrl
        {
            get; set;
        }

        public string RoyalBoxDevUrl
        {
            get; set;
        }

        public string BoomBoxUrl
        {
            get; set;
        }

        public string EventsBaseUrl
        {
            get; set;
        }

        public string EventsPostUrl
        {
            get; set;
        }

        public string EventsBaseStageUrl
        {
            get; set;
        }

        public string EventsPostStageUrl
        {
            get; set;
        }

    }
}