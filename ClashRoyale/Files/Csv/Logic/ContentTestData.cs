namespace ClashRoyale.Files.Csv.Logic
{
    public class ContentTestData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContentTestData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ContentTestData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ContentTestData.
        }

        public string SourceData { get; set; }

        public string TargetData { get; set; }

        public string Stat1 { get; set; }

        public string Operator { get; set; }

        public string Stat2 { get; set; }

        public int Result { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}