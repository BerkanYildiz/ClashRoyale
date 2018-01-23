namespace ClashRoyale.Files.Csv.Logic
{
    public class EventTargetingDefinitionData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventTargetingDefinitionData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventTargetingDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventTargetingDefinitionData.
        }

        public string MetadataType { get; set; }

        public string MetadataPath { get; set; }

        public string EvaluationLocation { get; set; }

        public string[] ParameterName { get; set; }

        public string[] ParameterType { get; set; }

        public bool[] IsRequired { get; set; }

        public string ObjectType { get; set; }

        public string[] MatchingRuleType { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}