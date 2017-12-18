namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class ConfigurationDefinitionData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDefinitionData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ConfigurationDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ConfigurationDefinitionData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string ObjectType
        {
            get; set;
        }

    }
}