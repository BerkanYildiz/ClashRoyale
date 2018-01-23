namespace ClashRoyale.Files.Csv.Logic
{
    public class QuestOrderData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QuestOrderData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public QuestOrderData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // QuestOrderData.
        }

        public string[] QuestType { get; set; }

        public int[] QuestCount { get; set; }

        public int DailyQuestPoints { get; set; }

        public string[] RewardType { get; set; }

        public int[] RewardCount { get; set; }

        public int[] RewardAmountSmall { get; set; }

        public int[] RewardAmountLarge { get; set; }

        public int[] QuestPointsSmall { get; set; }

        public int[] QuestPointsLarge { get; set; }

        public int[] RewardAmountDaily { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}