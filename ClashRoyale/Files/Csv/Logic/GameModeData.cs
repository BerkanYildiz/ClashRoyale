namespace ClashRoyale.Files.Csv.Logic
{
    public class GameModeData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GameModeData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public GameModeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // GameModeData.
        }

        public string TID { get; set; }

        public string RequestTID { get; set; }

        public string InProgressTID { get; set; }

        public string CardLevelAdjustment { get; set; }

        public int PlayerCount { get; set; }

        public string DeckSelection { get; set; }

        public int OvertimeSeconds { get; set; }

        public string[] PredefinedDecks { get; set; }

        public bool SameDeckOnBoth { get; set; }

        public bool SeparateTeamDecks { get; set; }

        public int ElixirProductionMultiplier { get; set; }

        public int ElixirProductionOvertimeMultiplier { get; set; }

        public bool UseStartingElixir { get; set; }

        public int StartingElixir { get; set; }

        public bool Heroes { get; set; }

        public string ForcedDeckCards { get; set; }

        public string Players { get; set; }

        public string EventDeckSetLimit { get; set; }

        public bool ForcedDeckCardsUsingCardTheme { get; set; }

        public string PrincessSkin { get; set; }

        public string KingSkin { get; set; }

        public bool GivesClanScore { get; set; }

        public int GoldPerTower1 { get; set; }

        public int GoldPerTower2 { get; set; }

        public int GoldPerTower3 { get; set; }

        public int GemsPerTower1 { get; set; }

        public int GemsPerTower2 { get; set; }

        public int GemsPerTower3 { get; set; }

        public string EndConfetti1 { get; set; }

        public string EndConfetti2 { get; set; }

        public int TargetTouchdowns { get; set; }

        public string SkinSet { get; set; }

        public bool FixedDeckOrder { get; set; }

        public string Icon { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}