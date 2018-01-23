namespace ClashRoyale.Files.Csv.Logic
{
    public class AllianceBadgeData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AllianceBadgeData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AllianceBadgeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AllianceBadgeData.
        }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public string Category { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}