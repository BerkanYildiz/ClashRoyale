namespace ClashRoyale.Files.Csv.Client
{
    public class SoundData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SoundData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SoundData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SoundData.
        }

        public string[] FileNames { get; set; }

        public int[] MinVolume { get; set; }

        public int[] MaxVolume { get; set; }

        public int[] MinPitch { get; set; }

        public int[] MaxPitch { get; set; }

        public int[] Priority { get; set; }

        public int[] MaximumByType { get; set; }

        public int[] MaxRepeatMs { get; set; }

        public bool Loop { get; set; }

        public bool PlayVariationsInSequence { get; set; }

        public bool PlayVariationsInSequenceManualReset { get; set; }

        public int[] StartDelayMinMs { get; set; }

        public int[] StartDelayMaxMs { get; set; }

        public bool PlayOnlyWhenInView { get; set; }

        public int MaxVolumeScaleLimit { get; set; }

        public int NoSoundScaleLimit { get; set; }

        public int PadEmpyToEndMs { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}