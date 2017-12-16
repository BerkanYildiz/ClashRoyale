namespace ClashRoyale.Server.Files.Csv.Logic
{
    internal class AreaEffectObjectData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="AreaEffectObjectData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AreaEffectObjectData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AreaEffectObjectData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string Rarity
        {
            get; set;
        }

        internal int LifeDuration
        {
            get; set;
        }

        internal int LifeDurationIncreasePerLevel
        {
            get; set;
        }

        internal int LifeDurationIncreaseAfterTournamentCap
        {
            get; set;
        }

        internal bool AffectsHidden
        {
            get; set;
        }

        internal int Radius
        {
            get; set;
        }

        internal string LoopingEffect
        {
            get; set;
        }

        internal string OneShotEffect
        {
            get; set;
        }

        internal string ScaledEffect
        {
            get; set;
        }

        internal string HitEffect
        {
            get; set;
        }

        internal int Pushback
        {
            get; set;
        }

        internal int MaximumTargets
        {
            get; set;
        }

        internal int HitSpeed
        {
            get; set;
        }

        internal int Damage
        {
            get; set;
        }

        internal bool NoEffectToCrownTowers
        {
            get; set;
        }

        internal int CrownTowerDamagePercent
        {
            get; set;
        }

        internal bool HitBiggestTargets
        {
            get; set;
        }

        internal string Buff
        {
            get; set;
        }

        internal int BuffTime
        {
            get; set;
        }

        internal int BuffTimeIncreasePerLevel
        {
            get; set;
        }

        internal int BuffTimeIncreaseAfterTournamentCap
        {
            get; set;
        }

        internal bool CapBuffTimeToAreaEffectTime
        {
            get; set;
        }

        internal int BuffNumber
        {
            get; set;
        }

        internal bool OnlyEnemies
        {
            get; set;
        }

        internal bool OnlyOwnTroops
        {
            get; set;
        }

        internal bool IgnoreBuildings
        {
            get; set;
        }

        internal string Projectile
        {
            get; set;
        }

        internal string SpawnCharacter
        {
            get; set;
        }

        internal int SpawnInterval
        {
            get; set;
        }

        internal bool SpawnRandomizeSequence
        {
            get; set;
        }

        internal string SpawnEffect
        {
            get; set;
        }

        internal string SpawnDeployBaseAnim
        {
            get; set;
        }

        internal int SpawnTime
        {
            get; set;
        }

        internal int SpawnCharacterLevelIndex
        {
            get; set;
        }

        internal int SpawnInitialDelay
        {
            get; set;
        }

        internal int SpawnMaxCount
        {
            get; set;
        }

        internal int SpawnMinRadius
        {
            get; set;
        }

        internal bool HitsGround
        {
            get; set;
        }

        internal bool HitsAir
        {
            get; set;
        }

        internal int ProjectileStartHeight
        {
            get; set;
        }

        internal bool ProjectilesToCenter
        {
            get; set;
        }

        internal string SpawnsAeo
        {
            get; set;
        }

        internal bool ControlsBuff
        {
            get; set;
        }

        internal bool Clone
        {
            get; set;
        }

        internal int AttractPercentage
        {
            get; set;
        }

    }
}