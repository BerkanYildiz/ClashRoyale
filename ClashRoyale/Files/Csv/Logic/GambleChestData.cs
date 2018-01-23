namespace ClashRoyale.Files.Csv.Logic
{
    public class GambleChestData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GambleChestData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public GambleChestData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // GambleChestData.
        }

        public int GoldPrice { get; set; }

        public string Location { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}