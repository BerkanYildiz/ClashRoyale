namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class LocationData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LocationData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public LocationData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // LocationData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal bool NpcOnly
        {
            get; set;
        }

        internal bool PvpOnly
        {
            get; set;
        }

        internal int ShadowR
        {
            get; set;
        }

        internal int ShadowG
        {
            get; set;
        }

        internal int ShadowB
        {
            get; set;
        }

        internal int ShadowA
        {
            get; set;
        }

        internal int ShadowOffsetX
        {
            get; set;
        }

        internal int ShadowOffsetY
        {
            get; set;
        }

        internal string Sound
        {
            get; set;
        }

        internal string ExtraTimeMusic
        {
            get; set;
        }

        internal int MatchLength
        {
            get; set;
        }

        internal string WinCondition
        {
            get; set;
        }

        internal int OvertimeSeconds
        {
            get; set;
        }

        internal int EndScreenDelay
        {
            get; set;
        }

        internal string FileName
        {
            get; set;
        }

        internal string TileDataFileName
        {
            get; set;
        }

        internal string AmbientSound
        {
            get; set;
        }

        internal string OverlaySc
        {
            get; set;
        }

        internal string OverlayExportName
        {
            get; set;
        }

        internal bool CrowdEffects
        {
            get; set;
        }

        internal string CloudFileName
        {
            get; set;
        }

        internal string CloudExportName
        {
            get; set;
        }

        internal bool CloudRandomFlip
        {
            get; set;
        }

        internal int CloudMinScale
        {
            get; set;
        }

        internal int CloudMaxScale
        {
            get; set;
        }

        internal int CloudMinSpeed
        {
            get; set;
        }

        internal int CloudMaxSpeed
        {
            get; set;
        }

        internal int CloudMinAlpha
        {
            get; set;
        }

        internal int CloudMaxAlpha
        {
            get; set;
        }

        internal int CloudCount
        {
            get; set;
        }

        internal string WalkEffect
        {
            get; set;
        }

        internal string WalkEffectOvertime
        {
            get; set;
        }

        internal string LoopingEffectRegularTime
        {
            get; set;
        }

        internal string LoopingEffectFinalMinute
        {
            get; set;
        }

        internal string LoopingEffectOvertime
        {
            get; set;
        }

        internal string LoopingEffect
        {
            get; set;
        }

        internal string LoopingEffectOvertimeSide
        {
            get; set;
        }

        internal int ReflectionRed
        {
            get; set;
        }

        internal int ReflectionGreen
        {
            get; set;
        }

        internal int ReflectionBlue
        {
            get; set;
        }

    }
}