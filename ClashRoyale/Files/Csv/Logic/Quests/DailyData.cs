namespace ClashRoyale.Files.Csv.Logic.Quests
{
    public class DailyData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DailyData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public DailyData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // DailyData.
        }

        public string Title { get; set; }

        public string Info { get; set; }

        public string ItemFile { get; set; }

        public string ItemExportName { get; set; }

        public int Count { get; set; }

        public int[] TimerHours { get; set; }

        public string[] RewardType { get; set; }

        public int Weight { get; set; }

        public string MinArena { get; set; }

        public string MaxArena { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}