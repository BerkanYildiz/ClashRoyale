namespace ClashRoyale.Files.Csv.Logic
{
    using System;
    using ClashRoyale.Enums;

    public class SpellData : CsvData
    {
        public RarityData RarityData;

        public DateTime ReleaseDateTime;
        public ArenaData UnlockArenaData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpellData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public SpellData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // SpellBuildingData.
        }

        /// <summary>
        ///     Gets the max level index.
        /// </summary>
        public int MaxLevelIndex
        {
            get
            {
                return this.RarityData.LevelCount - 1;
            }
        }

        public string IconSwf { get; set; }

        public string IconFile { get; set; }

        public string UnlockArena { get; set; }

        public string Rarity { get; set; }

        public int ManaCost { get; set; }

        public bool ManaCostFromSummonerMana { get; set; }

        public bool NotInUse { get; set; }

        public bool Mirror { get; set; }

        public int CustomDeployTime { get; set; }

        public string SummonCharacter { get; set; }

        public int SummonNumber { get; set; }

        public int SummonCharacterLevelIndex { get; set; }

        public string SummonCharacterSecond { get; set; }

        public int SummonCharacterSecondCount { get; set; }

        public int SummonRadius { get; set; }

        public int Radius { get; set; }

        public int Height { get; set; }

        public string Projectile { get; set; }

        public bool SpellAsDeploy { get; set; }

        public bool CanPlaceOnBuildings { get; set; }

        public int InstantDamage { get; set; }

        public int DurationSeconds { get; set; }

        public int InstantHeal { get; set; }

        public int HealPerSecond { get; set; }

        public string Effect { get; set; }

        public int Pushback { get; set; }

        public int MultipleProjectiles { get; set; }

        public string CustomFirstProjectile { get; set; }

        public int BuffTime { get; set; }

        public int BuffTimeIncreasePerLevel { get; set; }

        public int BuffNumber { get; set; }

        public string BuffType { get; set; }

        public string BuffOnDamage { get; set; }

        public bool OnlyOwnTroops { get; set; }

        public bool OnlyEnemies { get; set; }

        public bool CanDeployOnEnemySide { get; set; }

        public bool TouchdownLimitedDeploy { get; set; }

        public string CastSound { get; set; }

        public string AreaEffectObject { get; set; }

        public string Tid { get; set; }

        public string TidInfo { get; set; }

        public string IndicatorFileName { get; set; }

        public string IndicatorEffect { get; set; }

        public bool HideRadiusIndicator { get; set; }

        public string DestIndicatorEffect { get; set; }

        public string ReleaseDate { get; set; }

        public int ElixirProductionStopTime { get; set; }

        public bool DarkMirror { get; set; }

        public bool StatsUnderInfo { get; set; }

        public string HeroAbility { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            this.RarityData = CsvFiles.Get(Gamefile.Rarities).GetData<RarityData>(this.Rarity);

            if (this.RarityData == null)
            {
                throw new Exception("Spell " + this.GlobalId + " rarity is NULL.");
            }

            this.UnlockArenaData = CsvFiles.Get(Gamefile.Arenas).GetData<ArenaData>(this.UnlockArena);

            if (this.UnlockArenaData == null)
            {
                throw new Exception("UnlockArena not defined for spell " + this.GlobalId + ".");
            }

            if (!string.IsNullOrEmpty(this.ReleaseDate))
            {
                if (this.ReleaseDate == "Soon")
                {
                    this.ReleaseDateTime = new DateTime(2020, 1, 1);
                }
                else if (!DateTime.TryParse(this.ReleaseDate, out this.ReleaseDateTime))
                {
                    throw new Exception("ReleaseDate must be in format YYYY-MM-DD!");
                }
            }

            if (!this.NotInUse)
            {
                CsvFiles.Spells.Add(this);
            }
        }

        /// <summary>
        ///     Gets if the spell is unlocked for specified arena.
        /// </summary>
        public bool IsUnlockedInArena(ArenaData Data)
        {
            if (!this.UnlockArenaData.TrainingCamp && Data.TrainingCamp)
            {
                return false;
            }

            return Data.TrophyLimit >= this.UnlockArenaData.TrophyLimit;
        }
    }
}