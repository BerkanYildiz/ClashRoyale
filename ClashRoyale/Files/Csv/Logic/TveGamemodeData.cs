namespace ClashRoyale.Files.Csv.Logic
{
    public class TveGamemodeData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TveGamemodeData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TveGamemodeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TveGamemodeData.
        }

        public string[] PrimarySpells { get; set; }

        public string[] SecondarySpells { get; set; }

        public string[] CastSpells { get; set; }

        public bool RandomWaves { get; set; }

        public int ElixirPerWave { get; set; }

        public int WaveCount { get; set; }

        public int TimePerWave { get; set; }

        public int TimeToFirstWave { get; set; }

        public string[] ForcedCards1 { get; set; }

        public string[] ForcedCards2 { get; set; }

        public bool RotateDecks { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}