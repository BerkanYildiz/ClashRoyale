namespace ClashRoyale.Files.Csv.Logic
{
    public class QuestMainRewardData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QuestMainRewardData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public QuestMainRewardData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // QuestMainRewardData.
        }

        public string Chest { get; set; }

        public int QuestThreshold { get; set; }

        public string ShopChest { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}