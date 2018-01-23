namespace ClashRoyale.Files.Csv.Client
{
    public class BackgroundDecoData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BackgroundDecoData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public BackgroundDecoData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // BackgroundDecoData.
        }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string Layer { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}