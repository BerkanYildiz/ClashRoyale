namespace ClashRoyale.Client.Files.Csv.Client
{
    internal class HintData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="HintData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public HintData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // HintData.
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

        internal bool NotBeenInClan
        {
            get; set;
        }

        internal bool NotBeenInTournament
        {
            get; set;
        }

        internal bool NotCreatedTournament
        {
            get; set;
        }

        internal int MinNpcWins
        {
            get; set;
        }

        internal int MaxNpcWins
        {
            get; set;
        }

        internal int MinArena
        {
            get; set;
        }

        internal int MaxArena
        {
            get; set;
        }

        internal int MinTrophies
        {
            get; set;
        }

        internal int MaxTrophies
        {
            get; set;
        }

        internal int MinExpLevel
        {
            get; set;
        }

        internal int MaxExpLevel
        {
            get; set;
        }

        internal string Ostid
        {
            get; set;
        }

        internal string AndroidTid
        {
            get; set;
        }

    }
}