namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class ExpLevelData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ExpLevelData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ExpLevelData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ExpLevelData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int ExpToNextLevel
        {
            get; set;
        }

        internal int SummonerLevel
        {
            get; set;
        }

        internal int TowerLevel
        {
            get; set;
        }

        internal int TroopLevel
        {
            get; set;
        }

        internal int Decks
        {
            get; set;
        }

        internal int SummonerKillGold
        {
            get; set;
        }

        internal int TowerKillGold
        {
            get; set;
        }

        internal int DiamondReward
        {
            get; set;
        }

        internal int QuestSlots
        {
            get; set;
        }

    }
}