namespace ClashRoyale.Files.Csv.Logic.Quests
{
    public class PveData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PveData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public PveData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // PveData.
        }

        public string Title { get; set; }

        public string Info { get; set; }

        public string ItemFile { get; set; }

        public string ItemExportName { get; set; }

        public int Count { get; set; }

        public string Level { get; set; }

        public int Weight { get; set; }

        public string MinArena { get; set; }

        public string MaxArena { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}