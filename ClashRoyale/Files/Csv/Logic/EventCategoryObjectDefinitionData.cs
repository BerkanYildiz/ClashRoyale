namespace ClashRoyale.Files.Csv.Logic
{
    public class EventCategoryObjectDefinitionData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventCategoryObjectDefinitionData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryObjectDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryObjectDefinitionData.
        }

        public string[] PropertyName { get; set; }

        public string[] PropertyType { get; set; }

        public bool[] IsRequired { get; set; }

        public string[] ObjectType { get; set; }

        public int[] DefaultInt { get; set; }

        public string[] DefaultString { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}