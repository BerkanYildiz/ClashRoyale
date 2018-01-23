namespace ClashRoyale.Files.Csv.Logic.Quests
{
    public class CollectData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CollectData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public CollectData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // CollectData.
        }

        public int Size { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }

        public string ItemFile { get; set; }

        public string ItemExportName { get; set; }

        public int Count { get; set; }

        public string DataType { get; set; }

        public string Resource { get; set; }

        public string Rarity { get; set; }

        public string BaseChest { get; set; }

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