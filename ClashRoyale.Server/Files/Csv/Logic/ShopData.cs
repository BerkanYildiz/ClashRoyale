namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class ShopData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ShopData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ShopData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ShopData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Category
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

        internal string Rarity
        {
            get; set;
        }

        internal bool Disabled
        {
            get; set;
        }

        internal string Resource
        {
            get; set;
        }

        internal int Cost
        {
            get; set;
        }

        internal int Count
        {
            get; set;
        }

        internal int CycleDuration
        {
            get; set;
        }

        internal int CycleDeadzoneStart
        {
            get; set;
        }

        internal int CycleDeadzoneEnd
        {
            get; set;
        }

        internal bool TopSection
        {
            get; set;
        }

        internal bool SpecialOffer
        {
            get; set;
        }

        internal int DurationSecs
        {
            get; set;
        }

        internal int AvailabilitySecs
        {
            get; set;
        }

        internal bool SyncToShopCycle
        {
            get; set;
        }

        internal string Chest
        {
            get; set;
        }

        internal int TrophyLimit
        {
            get; set;
        }

        internal string Iap
        {
            get; set;
        }

        internal string StarterPackItem0Type
        {
            get; set;
        }

        internal string StarterPackItem0Id
        {
            get; set;
        }

        internal int StarterPackItem0Param1
        {
            get; set;
        }

        internal string StarterPackItem1Type
        {
            get; set;
        }

        internal string StarterPackItem1Id
        {
            get; set;
        }

        internal int StarterPackItem1Param1
        {
            get; set;
        }

        internal string StarterPackItem2Type
        {
            get; set;
        }

        internal string StarterPackItem2Id
        {
            get; set;
        }

        internal int StarterPackItem2Param1
        {
            get; set;
        }

        internal int ValueMultiplier
        {
            get; set;
        }

        internal bool AppendArenaToChestName
        {
            get; set;
        }

        internal string TiedToArenaUnlock
        {
            get; set;
        }

        internal string RepeatPurchaseGemPackOverride
        {
            get; set;
        }

        internal string EventName
        {
            get; set;
        }

        internal bool CostAdjustBasedOnChestContents
        {
            get; set;
        }

        internal bool IsChronosOffer
        {
            get; set;
        }

    }
}