namespace ClashRoyale.Files.Csv.Logic
{
    public class ExpLevelData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpLevelData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ExpLevelData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ExpLevelData.
        }

        public int ExpToNextLevel { get; set; }

        public int SummonerLevel { get; set; }

        public int TowerLevel { get; set; }

        public int TroopLevel { get; set; }

        public int Decks { get; set; }

        public int SummonerKillGold { get; set; }

        public int TowerKillGold { get; set; }

        public int DiamondReward { get; set; }

        public int QuestSlots { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}