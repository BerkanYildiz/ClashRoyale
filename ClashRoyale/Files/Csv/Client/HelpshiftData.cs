namespace ClashRoyale.Files.Csv.Client
{
    public class HelpshiftData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HelpshiftData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public HelpshiftData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // HelpshiftData.
        }

        public string HelpshiftId { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}