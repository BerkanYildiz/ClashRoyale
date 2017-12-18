namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class EventTargetingDefinitionData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="EventTargetingDefinitionData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventTargetingDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventTargetingDefinitionData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string MetadataType
        {
            get; set;
        }

        internal string MetadataPath
        {
            get; set;
        }

        internal string EvaluationLocation
        {
            get; set;
        }

        internal string ParameterName
        {
            get; set;
        }

        internal string ParameterType
        {
            get; set;
        }

        internal bool IsRequired
        {
            get; set;
        }

        internal string ObjectType
        {
            get; set;
        }

        internal string MatchingRuleType
        {
            get; set;
        }

    }
}