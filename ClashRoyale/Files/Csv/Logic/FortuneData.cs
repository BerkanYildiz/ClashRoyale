namespace ClashRoyale.Files.Csv.Logic
{
    public class FortuneData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FortuneData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public FortuneData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // FortuneData.
        }

        public int Chance { get; set; }

        public int NumUseful { get; set; }

        public int NumCatchup { get; set; }

        public int NumUpgraded { get; set; }

        public int LegendChance { get; set; }

        public int CatchupPoolSize { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}