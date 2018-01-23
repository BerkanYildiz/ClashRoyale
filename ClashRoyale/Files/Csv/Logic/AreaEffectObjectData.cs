namespace ClashRoyale.Files.Csv.Logic
{
    public class AreaEffectObjectData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AreaEffectObjectData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AreaEffectObjectData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AreaEffectObjectData.
        }

        public string Rarity { get; set; }

        public int LifeDuration { get; set; }

        public int LifeDurationIncreasePerLevel { get; set; }

        public int LifeDurationIncreaseAfterTournamentCap { get; set; }

        public bool AffectsHidden { get; set; }

        public int Radius { get; set; }

        public string LoopingEffect { get; set; }

        public string OneShotEffect { get; set; }

        public string ScaledEffect { get; set; }

        public string HitEffect { get; set; }

        public int Pushback { get; set; }

        public bool PushbackAll { get; set; }

        public int MinPushback { get; set; }

        public int MaximumTargets { get; set; }

        public int HitSpeed { get; set; }

        public int Damage { get; set; }

        public bool NoEffectToCrownTowers { get; set; }

        public int CrownTowerDamagePercent { get; set; }

        public bool HitBiggestTargets { get; set; }

        public string Buff { get; set; }

        public int BuffTime { get; set; }

        public int BuffTimeIncreasePerLevel { get; set; }

        public int BuffTimeIncreaseAfterTournamentCap { get; set; }

        public bool CapBuffTimeToAreaEffectTime { get; set; }

        public int BuffNumber { get; set; }

        public bool OnlyEnemies { get; set; }

        public bool OnlyOwnTroops { get; set; }

        public bool IgnoreBuildings { get; set; }

        public string Projectile { get; set; }

        public string SpawnCharacter { get; set; }

        public int SpawnInterval { get; set; }

        public bool SpawnRandomizeSequence { get; set; }

        public string SpawnEffect { get; set; }

        public string SpawnDeployBaseAnim { get; set; }

        public int SpawnTime { get; set; }

        public int SpawnCharacterLevelIndex { get; set; }

        public int SpawnInitialDelay { get; set; }

        public int SpawnMaxCount { get; set; }

        public int SpawnMinRadius { get; set; }

        public bool HitsGround { get; set; }

        public bool HitsAir { get; set; }

        public int ProjectileStartHeight { get; set; }

        public bool ProjectilesToCenter { get; set; }

        public string SpawnsAEO { get; set; }

        public bool ControlsBuff { get; set; }

        public bool Clone { get; set; }

        public int AttractPercentage { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}