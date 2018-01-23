namespace ClashRoyale.Files.Csv.Client
{
    public class HintData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HintData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public HintData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // HintData.
        }

        public string TID { get; set; }

        public bool NotBeenInClan { get; set; }

        public bool NotBeenInTournament { get; set; }

        public bool NotCreatedTournament { get; set; }

        public int MinNpcWins { get; set; }

        public int MaxNpcWins { get; set; }

        public int MinArena { get; set; }

        public int MaxArena { get; set; }

        public int MinTrophies { get; set; }

        public int MaxTrophies { get; set; }

        public int MinExpLevel { get; set; }

        public int MaxExpLevel { get; set; }

        public string iOSTID { get; set; }

        public string AndroidTID { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}