namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class GameModeData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="GameModeData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public GameModeData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // GameModeData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Tid
        {
            get; set;
        }

        internal string RequestTid
        {
            get; set;
        }

        internal string InProgressTid
        {
            get; set;
        }

        internal string CardLevelAdjustment
        {
            get; set;
        }

        internal int PlayerCount
        {
            get; set;
        }

        internal string DeckSelection
        {
            get; set;
        }

        internal int OvertimeSeconds
        {
            get; set;
        }

        internal string PredefinedDecks
        {
            get; set;
        }

        internal bool SameDeckOnBoth
        {
            get; set;
        }

        internal bool SeparateTeamDecks
        {
            get; set;
        }

        internal int ElixirProductionMultiplier
        {
            get; set;
        }

        internal int ElixirProductionOvertimeMultiplier
        {
            get; set;
        }

        internal bool UseStartingElixir
        {
            get; set;
        }

        internal int StartingElixir
        {
            get; set;
        }

        internal bool Heroes
        {
            get; set;
        }

        internal string ForcedDeckCards
        {
            get; set;
        }

        internal string Players
        {
            get; set;
        }

        internal string EventDeckSetLimit
        {
            get; set;
        }

        internal bool ForcedDeckCardsUsingCardTheme
        {
            get; set;
        }

        internal string PrincessSkin
        {
            get; set;
        }

        internal string KingSkin
        {
            get; set;
        }

        internal bool GivesClanScore
        {
            get; set;
        }

        internal int GoldPerTower1
        {
            get; set;
        }

        internal int GoldPerTower2
        {
            get; set;
        }

        internal int GoldPerTower3
        {
            get; set;
        }

        internal int GemsPerTower1
        {
            get; set;
        }

        internal int GemsPerTower2
        {
            get; set;
        }

        internal int GemsPerTower3
        {
            get; set;
        }

        internal string EndConfetti1
        {
            get; set;
        }

        internal string EndConfetti2
        {
            get; set;
        }

        internal int TargetTouchdowns
        {
            get; set;
        }

        internal string SkinSet
        {
            get; set;
        }

        internal bool FixedDeckOrder
        {
            get; set;
        }

        internal string Icon
        {
            get; set;
        }

    }
}