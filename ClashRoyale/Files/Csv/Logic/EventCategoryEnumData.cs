namespace ClashRoyale.Files.Csv.Logic
{
    public class EventCategoryEnumData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventCategoryEnumData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryEnumData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryEnumData.
        }

        public string[] Option { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}