namespace ClashRoyale.Files.Csv.Logic
{
    public class SurvivalModeData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SurvivalModeData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SurvivalModeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SurvivalModeData.
        }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public string GameMode { get; set; }

        public string WinsIconExportName { get; set; }

        public bool Enabled { get; set; }

        public bool EventOnly { get; set; }

        public int JoinCost { get; set; }

        public string JoinCostResource { get; set; }

        public int FreePass { get; set; }

        public int MaxWins { get; set; }

        public int MaxLoss { get; set; }

        public int[] RewardCards { get; set; }

        public int[] RewardGold { get; set; }

        public int[] RewardSpellCount { get; set; }

        public string[] RewardSpell { get; set; }

        public int RewardSpellMaxCount { get; set; }

        public string ItemExportName { get; set; }

        public string ConfirmExportName { get; set; }

        public string TID { get; set; }

        public string CardTheme { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}