namespace ClashRoyale.Server.Files.Csv.Logic.Quests
{
    internal class CastData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="CastData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public CastData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // CastData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int Size
        {
            get; set;
        }

        internal string Title
        {
            get; set;
        }

        internal string Info
        {
            get; set;
        }

        internal string ItemFile
        {
            get; set;
        }

        internal string ItemExportName
        {
            get; set;
        }

        internal int Count
        {
            get; set;
        }

        internal string GameType
        {
            get; set;
        }

        internal string PlayerType
        {
            get; set;
        }

        internal string Spell
        {
            get; set;
        }

        internal string Rarity
        {
            get; set;
        }

        internal string Type
        {
            get; set;
        }

        internal int MinElixir
        {
            get; set;
        }

        internal int MaxElixir
        {
            get; set;
        }

        internal int Weight
        {
            get; set;
        }

        internal string MinArena
        {
            get; set;
        }

        internal string MaxArena
        {
            get; set;
        }

    }
}