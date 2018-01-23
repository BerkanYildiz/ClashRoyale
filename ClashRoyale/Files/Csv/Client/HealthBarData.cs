namespace ClashRoyale.Files.Csv.Client
{
    public class HealthBarData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthBarData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public HealthBarData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // HealthBarData.
        }

        public string FileName { get; set; }

        public string PlayerExportName { get; set; }

        public string EnemyExportName { get; set; }

        public string NoDamagePlayerExportName { get; set; }

        public string NoDamageEnemyExportName { get; set; }

        public int MinimumHitpointValue { get; set; }

        public bool ShowOwnAlways { get; set; }

        public bool ShowEnemyAlways { get; set; }

        public int YOffset { get; set; }

        public bool ShowAsShield { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}