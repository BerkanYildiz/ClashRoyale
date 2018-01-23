namespace ClashRoyale.Files.Csv.Logic
{
    public class SkinData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SkinData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SkinData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SkinData.
        }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string ExportNameRed { get; set; }

        public string TopExportName { get; set; }

        public string TopExportNameRed { get; set; }

        public int ValueGems { get; set; }

        public string TID { get; set; }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public bool IsInUse { get; set; }

        public string DeathEffect { get; set; }

        public string DeathEffect2 { get; set; }

        public bool EventSkin { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}