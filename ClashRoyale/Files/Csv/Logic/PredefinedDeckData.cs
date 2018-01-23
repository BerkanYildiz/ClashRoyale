namespace ClashRoyale.Files.Csv.Logic
{
    public class PredefinedDeckData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PredefinedDeckData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public PredefinedDeckData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // PredefinedDeckData.
        }

        public string[] Spells { get; set; }

        public int[] SpellLevel { get; set; }

        public string[] RandomSpellSets { get; set; }

        public string Description { get; set; }

        public string TID { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}