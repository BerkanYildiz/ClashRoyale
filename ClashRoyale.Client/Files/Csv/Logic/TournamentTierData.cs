namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class TournamentTierData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="TournamentTierData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TournamentTierData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TournamentTierData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int Version
        {
            get; set;
        }

        internal bool Disabled
        {
            get; set;
        }

        internal int CreateCost
        {
            get; set;
        }

        internal int MaxPlayers
        {
            get; set;
        }

        internal int Prize1
        {
            get; set;
        }

        internal int Prize2
        {
            get; set;
        }

        internal int Prize3
        {
            get; set;
        }

        internal int Prize10
        {
            get; set;
        }

        internal int Prize20
        {
            get; set;
        }

        internal int Prize30
        {
            get; set;
        }

        internal int Prize40
        {
            get; set;
        }

        internal int Prize50
        {
            get; set;
        }

        internal int Prize60
        {
            get; set;
        }

        internal int Prize70
        {
            get; set;
        }

        internal int Prize80
        {
            get; set;
        }

        internal int Prize90
        {
            get; set;
        }

        internal int Prize100
        {
            get; set;
        }

        internal int Prize150
        {
            get; set;
        }

        internal int Prize200
        {
            get; set;
        }

        internal int Prize250
        {
            get; set;
        }

        internal int Prize300
        {
            get; set;
        }

        internal int Prize350
        {
            get; set;
        }

        internal int Prize400
        {
            get; set;
        }

        internal int Prize450
        {
            get; set;
        }

        internal int Prize500
        {
            get; set;
        }

        internal int OpenChestVariation
        {
            get; set;
        }

    }
}