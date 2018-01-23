namespace ClashRoyale.Files.Csv.Logic
{
    public class ShopChestData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopChestData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ShopChestData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ShopChestData.
        }

        public string Type { get; set; }

        public string MinArena { get; set; }

        public string MaxArena { get; set; }

        public string MechanicUnlock { get; set; }

        public int Arena { get; set; }

        public int Price { get; set; }

        public bool DailyCardRotation { get; set; }

        public int[] DailyCardRotationChances { get; set; }

        public bool GuarenteedLegendary { get; set; }

        public int NumRerolls { get; set; }

        public string ShopPopupName { get; set; }

        public string ShopContainerName { get; set; }

        public string PopupContainerName { get; set; }

        public string ChestOpeningContainerName { get; set; }

        public string QuestContainerName { get; set; }

        public string AnimationPrefix { get; set; }

        public bool HighlightWhenUnlocked { get; set; }

        public string ChinaShopPopupName { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}