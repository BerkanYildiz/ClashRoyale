namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class HealthBarData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthBarData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public HealthBarData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // HealthBarData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string FileName
        {
            get; set;
        }

        internal string PlayerExportName
        {
            get; set;
        }

        internal string EnemyExportName
        {
            get; set;
        }

        internal string NoDamagePlayerExportName
        {
            get; set;
        }

        internal string NoDamageEnemyExportName
        {
            get; set;
        }

        internal int MinimumHitpointValue
        {
            get; set;
        }

        internal bool ShowOwnAlways
        {
            get; set;
        }

        internal bool ShowEnemyAlways
        {
            get; set;
        }

        internal int YOffset
        {
            get; set;
        }

        internal bool ShowAsShield
        {
            get; set;
        }

    }
}