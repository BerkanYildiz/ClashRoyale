namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class EventCategoryObjectDefinitionData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="EventCategoryObjectDefinitionData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EventCategoryObjectDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EventCategoryObjectDefinitionData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string PropertyName
        {
            get; set;
        }

        internal string PropertyType
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

        internal int DefaultInt
        {
            get; set;
        }

        internal string DefaultString
        {
            get; set;
        }

    }
}