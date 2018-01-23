namespace ClashRoyale.Files.Csv.Logic
{
    public class NpcData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NpcData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public NpcData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // NpcData.
        }

        public string Location { get; set; }

        public string PredefinedDeck { get; set; }

        public int Trophies { get; set; }

        public int ManaRegenMs { get; set; }

        public int ManaRegenMsEnd { get; set; }

        public int ManaRegenMsOvertime { get; set; }

        public int ExpLevel { get; set; }

        public bool CanReplay { get; set; }

        public string TID { get; set; }

        public int ExpReward { get; set; }

        public int Seed { get; set; }

        public bool FullDeckNotNeeded { get; set; }

        public int ManaReserve { get; set; }

        public int StartingMana { get; set; }

        public int WizardHpMultiplier { get; set; }

        public string StartTaunt { get; set; }

        public string OwnTowerDestroyedTaunt { get; set; }

        public bool HighlightTargetsOnManaFull { get; set; }

        public bool TrainingMatchAllowed { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}