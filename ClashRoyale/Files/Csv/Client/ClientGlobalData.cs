namespace ClashRoyale.Files.Csv.Client
{
    public class ClientGlobalData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientGlobalData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ClientGlobalData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ClientGlobalData.
        }

        public int NumberValue { get; set; }

        public bool BooleanValue { get; set; }

        public string TextValue { get; set; }

        public string[] StringArray { get; set; }

        public int[] NumberArray { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}