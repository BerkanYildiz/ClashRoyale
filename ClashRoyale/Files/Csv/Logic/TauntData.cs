namespace ClashRoyale.Files.Csv.Logic
{
    public class TauntData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TauntData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TauntData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TauntData.
        }

        public string TID { get; set; }

        public bool TauntMenu { get; set; }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string IconExportName { get; set; }

        public string BtnExportName { get; set; }

        public string Sound { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}