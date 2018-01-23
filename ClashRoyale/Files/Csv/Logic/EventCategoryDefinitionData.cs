namespace ClashRoyale.Files.Csv.Logic
{
    public class EventCategoryDefinitionData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventCategoryDefinitionData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryDefinitionData.
        }

        public string ObjectType { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}