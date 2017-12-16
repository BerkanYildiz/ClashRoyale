namespace ClashRoyale.Server.Files.Csv.Client
{
    internal class MusicData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="MusicData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public MusicData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // MusicData.
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

        internal int Volume
        {
            get; set;
        }

        internal bool Loop
        {
            get; set;
        }

        internal int PlayCount
        {
            get; set;
        }

        internal int FadeOutTimeSec
        {
            get; set;
        }

        internal int DurationSec
        {
            get; set;
        }

    }
}