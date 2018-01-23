namespace ClashRoyale.Files.Csv.Logic
{
    public class LocationData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LocationData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public LocationData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // LocationData.
        }

        public bool NpcOnly { get; set; }

        public bool PvpOnly { get; set; }

        public int ShadowR { get; set; }

        public int ShadowG { get; set; }

        public int ShadowB { get; set; }

        public int ShadowA { get; set; }

        public int ShadowOffsetX { get; set; }

        public int ShadowOffsetY { get; set; }

        public string Sound { get; set; }

        public string ExtraTimeMusic { get; set; }

        public int MatchLength { get; set; }

        public string WinCondition { get; set; }

        public int OvertimeSeconds { get; set; }

        public int EndScreenDelay { get; set; }

        public string FileName { get; set; }

        public string TileDataFileName { get; set; }

        public string AmbientSound { get; set; }

        public string OverlaySC { get; set; }

        public string OverlayExportName { get; set; }

        public bool CrowdEffects { get; set; }

        public string CloudFileName { get; set; }

        public string CloudExportName { get; set; }

        public bool CloudRandomFlip { get; set; }

        public int CloudMinScale { get; set; }

        public int CloudMaxScale { get; set; }

        public int CloudMinSpeed { get; set; }

        public int CloudMaxSpeed { get; set; }

        public int CloudMinAlpha { get; set; }

        public int CloudMaxAlpha { get; set; }

        public int CloudCount { get; set; }

        public string WalkEffect { get; set; }

        public string WalkEffectOvertime { get; set; }

        public string LoopingEffectRegularTime { get; set; }

        public string LoopingEffectFinalMinute { get; set; }

        public string LoopingEffectOvertime { get; set; }

        public string LoopingEffect { get; set; }

        public string LoopingEffectOvertimeSide { get; set; }

        public int ReflectionRed { get; set; }

        public int ReflectionGreen { get; set; }

        public int ReflectionBlue { get; set; }

        public string ChainedSWF { get; set; }

        public string ChainedExportName { get; set; }

        public string MusicOverride { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}