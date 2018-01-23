namespace ClashRoyale.Files.Csv.Logic
{
    public class DecoData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DecoData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public DecoData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // DecoData.
        }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string Layer { get; set; }

        public string LowendLayer { get; set; }

        public int ShadowScaleX { get; set; }

        public int ShadowScaleY { get; set; }

        public int ShadowX { get; set; }

        public int ShadowY { get; set; }

        public int ShadowSkew { get; set; }

        public int CollisionRadius { get; set; }

        public string Effect { get; set; }

        public string[] AssetMinTrophy { get; set; }

        public int[] AssetMinTrophyScore { get; set; }

        public string[] AssetMinTrophyFileName { get; set; }

        public int SortValue { get; set; }

        public bool Audience { get; set; }

        public string CheerFileName { get; set; }

        public string CheerExportName { get; set; }

        public string ClassType { get; set; }

        public bool Animate { get; set; }

        public int StartFrameOverride { get; set; }

        public string Visibility { get; set; }

        public string ChainedSWF { get; set; }

        public string ChainedExportName { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}