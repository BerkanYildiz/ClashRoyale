namespace ClashRoyale.Files.Csv.Logic
{
    public class EventCategoryData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventCategoryData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryData.
        }

        public string CSVFiles { get; set; }

        public string[] CSVRows { get; set; }

        public string[] CustomNames { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}