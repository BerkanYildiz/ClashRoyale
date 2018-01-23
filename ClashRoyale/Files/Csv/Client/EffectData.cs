namespace ClashRoyale.Files.Csv.Client
{
    public class EffectData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EffectData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public EffectData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // EffectData.
        }

        public bool Loop { get; set; }

        public bool FollowParent { get; set; }

        public int[] ShakeScreen { get; set; }

        public int ShakeMaxIntensity { get; set; }

        public int[] Time { get; set; }

        public int[] RenderableScale { get; set; }

        public string[] Sound { get; set; }

        public string[] Type { get; set; }

        public string[] FileName { get; set; }

        public string[] ExportName { get; set; }

        public string[] ParticleEmitterName { get; set; }

        public string[] Effect { get; set; }

        public string[] Layer { get; set; }

        public int[] Scale { get; set; }

        public string[] TextInstanceName { get; set; }

        public string[] TextParentInstanceName { get; set; }

        public string EnemyVersion { get; set; }

        public int FlashWidth { get; set; }

        public bool KillLoopingSoundsOnEnd { get; set; }

        public string[] OutputEvent { get; set; }

        public int ParentLookAtOffsetRadius { get; set; }

        public int ForcedLength { get; set; }

        public bool Shadow { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}