namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class CharacterBuffData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="CharacterBuffData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public CharacterBuffData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // CharacterBuffData.
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

        internal string Tid
        {
            get; set;
        }

        internal string IconFileName
        {
            get; set;
        }

        internal string IconExportName
        {
            get; set;
        }

        internal bool ChangeControl
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

        internal int DamagePerSecond
        {
            get; set;
        }

        internal int HitFrequency
        {
            get; set;
        }

        internal int DamageReduction
        {
            get; set;
        }

        internal int HealPerSecond
        {
            get; set;
        }

        internal int HitSpeedMultiplier
        {
            get; set;
        }

        internal int SpeedMultiplier
        {
            get; set;
        }

        internal int SpawnSpeedMultiplier
        {
            get; set;
        }

        internal string NegatesBuffs
        {
            get; set;
        }

        internal string ImmunityToBuffs
        {
            get; set;
        }

        internal bool Invisible
        {
            get; set;
        }

        internal bool ImmuneToSpells
        {
            get; set;
        }

        internal bool RemoveOnAttack
        {
            get; set;
        }

        internal bool RemoveOnHeal
        {
            get; set;
        }

        internal int DamageMultiplier
        {
            get; set;
        }

        internal bool Panic
        {
            get; set;
        }

        internal string Effect
        {
            get; set;
        }

        internal string FilterFile
        {
            get; set;
        }

        internal string FilterExportName
        {
            get; set;
        }

        internal bool FilterAffectsTransformation
        {
            get; set;
        }

        internal bool FilterInheritLifeDuration
        {
            get; set;
        }

        internal int SizeMultiplier
        {
            get; set;
        }

        internal bool StaticTarget
        {
            get; set;
        }

        internal bool IgnorePushBack
        {
            get; set;
        }

        internal string MarkEffect
        {
            get; set;
        }

        internal int AudioPitchModifier
        {
            get; set;
        }

        internal string PortalSpell
        {
            get; set;
        }

        internal int AttractPercentage
        {
            get; set;
        }

        internal bool ControlledByParent
        {
            get; set;
        }

        internal bool Clone
        {
            get; set;
        }

        internal int Scale
        {
            get; set;
        }

        internal bool EnableStacking
        {
            get; set;
        }

        internal string ChainedBuff
        {
            get; set;
        }

        internal string ContinuousEffect
        {
            get; set;
        }

    }
}