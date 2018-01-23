namespace ClashRoyale.Files.Csv.Logic
{
    public class ConfigurationDefinitionData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigurationDefinitionData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ConfigurationDefinitionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ConfigurationDefinitionData.
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