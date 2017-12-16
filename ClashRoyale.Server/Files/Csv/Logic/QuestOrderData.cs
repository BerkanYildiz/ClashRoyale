namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class QuestOrderData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="QuestOrderData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public QuestOrderData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // QuestOrderData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string QuestType
        {
            get; set;
        }

        internal int QuestCount
        {
            get; set;
        }

        internal int DailyChance
        {
            get; set;
        }

        internal int DailyQuestPoints
        {
            get; set;
        }

        internal string RewardType
        {
            get; set;
        }

        internal int RewardCount
        {
            get; set;
        }

        internal int RewardAmountSmall
        {
            get; set;
        }

        internal int RewardAmountLarge
        {
            get; set;
        }

        internal int QuestPointsSmall
        {
            get; set;
        }

        internal int QuestPointsLarge
        {
            get; set;
        }

        internal int RewardAmountDaily
        {
            get; set;
        }

    }
}