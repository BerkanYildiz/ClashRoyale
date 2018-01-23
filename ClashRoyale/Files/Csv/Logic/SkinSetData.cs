namespace ClashRoyale.Files.Csv.Logic
{
    public class SkinSetData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SkinSetData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SkinSetData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SkinSetData.
        }

        public string Character { get; set; }

        public string Skin { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}