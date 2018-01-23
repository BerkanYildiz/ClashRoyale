namespace ClashRoyale.Files.Csv.Logic
{
    public class RegionData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public RegionData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // RegionData.
        }

        public string TID { get; set; }

        public string DisplayName { get; set; }

        public bool IsCountry { get; set; }

        public bool RegionPopup { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}