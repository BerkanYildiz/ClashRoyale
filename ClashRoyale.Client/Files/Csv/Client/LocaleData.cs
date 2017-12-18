namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class LocaleData : CsvData
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
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal bool Enabled
        {
            get; set;
        }

        internal string Description
        {
            get; set;
        }

        internal int SortOrder
        {
            get; set;
        }

        internal bool HasEvenSpaceCharacters
        {
            get; set;
        }

        internal string UsedSystemFont
        {
            get; set;
        }

        internal string HelpshiftSdkLanguage
        {
            get; set;
        }

        internal string HelpshiftSdkLanguageAndroid
        {
            get; set;
        }

        internal string HelpshiftLanguageTag
        {
            get; set;
        }

        internal string TermsAndServiceUrl
        {
            get; set;
        }

        internal string TournamentTermsUrl
        {
            get; set;
        }

        internal string ParentsGuideUrl
        {
            get; set;
        }

        internal string PrivacyPolicyUrl
        {
            get; set;
        }

        internal bool TestLanguage
        {
            get; set;
        }

        internal string TestExcludes
        {
            get; set;
        }

        internal string RegionListFile
        {
            get; set;
        }

        internal bool MaintenanceRoyalBox
        {
            get; set;
        }

        internal string RoyalBoxUrl
        {
            get; set;
        }

        internal string RoyalBoxStageUrl
        {
            get; set;
        }

        internal string RoyalBoxDevUrl
        {
            get; set;
        }

        internal string BoomBoxUrl
        {
            get; set;
        }

        internal string EventsBaseUrl
        {
            get; set;
        }

        internal string EventsPostUrl
        {
            get; set;
        }

        internal string EventsBaseStageUrl
        {
            get; set;
        }

        internal string EventsPostStageUrl
        {
            get; set;
        }

    }
}