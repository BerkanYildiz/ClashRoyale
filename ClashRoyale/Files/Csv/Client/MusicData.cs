namespace ClashRoyale.Files.Csv.Client
{
    public class MusicData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MusicData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public MusicData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // MusicData.
        }

        public string[] FileName { get; set; }

        public int[] Volume { get; set; }

        public bool[] Loop { get; set; }

        public int[] PlayCount { get; set; }

        public int[] FadeOutTimeSec { get; set; }

        public int[] DurationSec { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}