namespace ClashRoyale.Files.Csv.Client
{
    public class NewsData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NewsData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public NewsData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // NewsData.
        }

        public int ID { get; set; }

        public bool Enabled { get; set; }

        public string TID { get; set; }

        public string InfoTID { get; set; }

        public string ItemSWF { get; set; }

        public string ItemExportName { get; set; }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public string ImageSWF { get; set; }

        public string ImageExportName { get; set; }

        public string ButtonUrl { get; set; }

        public string ButtonTID { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}