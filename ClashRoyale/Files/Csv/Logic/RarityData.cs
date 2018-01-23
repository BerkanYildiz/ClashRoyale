namespace ClashRoyale.Files.Csv.Logic
{
    public class RarityData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RarityData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public RarityData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // RarityData.
        }

        public int LevelCount { get; set; }

        public int RelativeLevel { get; set; }

        public int MirrorRelativeLevel { get; set; }

        public int CloneRelativeLevel { get; set; }

        public int DonateCapacity { get; set; }

        public int SortCapacity { get; set; }

        public int DonateReward { get; set; }

        public int DonateXp { get; set; }

        public int GoldConversionValue { get; set; }

        public int ChanceWeight { get; set; }

        public int BalanceMultiplier { get; set; }

        public int[] UpgradeExp { get; set; }

        public int[] UpgradeMaterialCount { get; set; }

        public int[] UpgradeCost { get; set; }

        public int PowerLevelMultiplier { get; set; }

        public int RefundGems { get; set; }

        public string Tid { get; set; }

        public string CardBaseFileName { get; set; }

        public string BigFrameExportName { get; set; }

        public string CardBaseExportName { get; set; }

        public string StackedCardExportName { get; set; }

        public string CardRewardExportName { get; set; }

        public string CastEffect { get; set; }

        public string InfoTitleExportName { get; set; }

        public string CardRarityBgExportName { get; set; }

        public int SortOrder { get; set; }

        public int Red { get; set; }

        public int Green { get; set; }

        public int Blue { get; set; }

        public string AppearEffect { get; set; }

        public string BuySound { get; set; }

        public string LoopEffect { get; set; }

        public int CardTxtBgFrameIdx { get; set; }

        public string CardGlowInstanceName { get; set; }

        public string SpellSelectedSound { get; set; }

        public string SpellAvailableSound { get; set; }

        public string RotateExportName { get; set; }

        public string IconSwf { get; set; }

        public string IconExportName { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            if (this.Name == "Common")
            {
                CsvFiles.RarityCommonData = this;
            }
        }
    }
}