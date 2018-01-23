namespace ClashRoyale.Files.Csv.Logic
{
    public class TutorialChestOrderData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TutorialChestOrderData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TutorialChestOrderData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TutorialChestOrderData.
        }

        public string[] Chest { get; set; }

        public string[] NPC { get; set; }

        public string[] PvETutorial { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}