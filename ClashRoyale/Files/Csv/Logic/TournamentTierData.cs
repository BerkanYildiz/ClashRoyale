namespace ClashRoyale.Files.Csv.Logic
{
    public class TournamentTierData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TournamentTierData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TournamentTierData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TournamentTierData.
        }

        public int Version { get; set; }

        public bool Disabled { get; set; }

        public int CreateCost { get; set; }

        public int MaxPlayers { get; set; }

        public int Prize1 { get; set; }

        public int Prize2 { get; set; }

        public int Prize3 { get; set; }

        public int Prize10 { get; set; }

        public int Prize20 { get; set; }

        public int Prize30 { get; set; }

        public int Prize40 { get; set; }

        public int Prize50 { get; set; }

        public int Prize60 { get; set; }

        public int Prize70 { get; set; }

        public int Prize80 { get; set; }

        public int Prize90 { get; set; }

        public int Prize100 { get; set; }

        public int Prize150 { get; set; }

        public int Prize200 { get; set; }

        public int Prize250 { get; set; }

        public int Prize300 { get; set; }

        public int Prize350 { get; set; }

        public int Prize400 { get; set; }

        public int Prize450 { get; set; }

        public int Prize500 { get; set; }

        public int OpenChestVariation { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}