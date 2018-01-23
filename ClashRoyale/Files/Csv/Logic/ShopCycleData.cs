namespace ClashRoyale.Files.Csv.Logic
{
    public class ShopCycleData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopCycleData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ShopCycleData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ShopCycleData.
        }

        public int[] A { get; set; }

        public int[] B { get; set; }

        public int[] C { get; set; }

        public string[] D { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}