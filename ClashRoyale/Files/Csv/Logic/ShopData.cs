namespace ClashRoyale.Files.Csv.Logic
{
    public class ShopData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ShopData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ShopData.
        }

        public string Category { get; set; }

        public string TID { get; set; }

        public string Rarity { get; set; }

        public bool Disabled { get; set; }

        public string Resource { get; set; }

        public int Cost { get; set; }

        public int Count { get; set; }

        public int CycleDuration { get; set; }

        public int CycleDeadzoneStart { get; set; }

        public int CycleDeadzoneEnd { get; set; }

        public bool TopSection { get; set; }

        public bool SpecialOffer { get; set; }

        public int DurationSecs { get; set; }

        public int AvailabilitySecs { get; set; }

        public bool SyncToShopCycle { get; set; }

        public string Chest { get; set; }

        public int TrophyLimit { get; set; }

        public string IAP { get; set; }

        public string StarterPackItem0Type { get; set; }

        public string StarterPackItem0ID { get; set; }

        public int StarterPackItem0Param1 { get; set; }

        public string StarterPackItem1Type { get; set; }

        public string StarterPackItem1ID { get; set; }

        public int StarterPackItem1Param1 { get; set; }

        public string StarterPackItem2Type { get; set; }

        public string StarterPackItem2ID { get; set; }

        public int StarterPackItem2Param1 { get; set; }

        public int ValueMultiplier { get; set; }

        public bool AppendArenaToChestName { get; set; }

        public string TiedToArenaUnlock { get; set; }

        public string RepeatPurchaseGemPackOverride { get; set; }

        public string EventName { get; set; }

        public bool CostAdjustBasedOnChestContents { get; set; }

        public bool IsChronosOffer { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}