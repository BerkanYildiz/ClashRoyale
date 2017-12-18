namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class PveGamemodeData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="PveGamemodeData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public PveGamemodeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // PveGamemodeData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string VictoryCondition
        {
            get; set;
        }

        internal string ForcedCards
        {
            get; set;
        }

        internal string Location
        {
            get; set;
        }

        internal string ComputerPlayerType
        {
            get; set;
        }

        internal string TowerRules
        {
            get; set;
        }

        internal string WaveSpell
        {
            get; set;
        }

        internal int WaveSpellLevelIndex
        {
            get; set;
        }

        internal int WaveDelay
        {
            get; set;
        }

        internal int WaveX
        {
            get; set;
        }

        internal int WaveY
        {
            get; set;
        }

        internal bool WaveRepeat
        {
            get; set;
        }

        internal int WaveRepeatTime
        {
            get; set;
        }

    }
}