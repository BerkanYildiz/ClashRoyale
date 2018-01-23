namespace ClashRoyale.Files.Csv.Logic
{
    public class PveGamemodeData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PveGamemodeData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public PveGamemodeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // PveGamemodeData.
        }

        public string VictoryCondition { get; set; }

        public string[] ForcedCards { get; set; }

        public string Location { get; set; }

        public string ComputerPlayerType { get; set; }

        public string TowerRules { get; set; }

        public string[] WaveSpell { get; set; }

        public int WaveSpellLevelIndex { get; set; }

        public int[] WaveDelay { get; set; }

        public int[] WaveX { get; set; }

        public int[] WaveY { get; set; }

        public bool[] WaveRepeat { get; set; }

        public int[] WaveRepeatTime { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}