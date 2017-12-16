namespace ClashRoyale.Server.Files.Csv.Client
{
    internal class ParticleEmitterData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ParticleEmitterData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ParticleEmitterData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int ParticleCount
        {
            get; set;
        }

        internal int MinLife
        {
            get; set;
        }

        internal int MaxLife
        {
            get; set;
        }

        internal int ParticleMinInterval
        {
            get; set;
        }

        internal int ParticleMaxInterval
        {
            get; set;
        }

        internal int ParticleMinLife
        {
            get; set;
        }

        internal int ParticleMaxLife
        {
            get; set;
        }

        internal int ParticleMinAngle
        {
            get; set;
        }

        internal int ParticleMaxAngle
        {
            get; set;
        }

        internal bool ParticleAngleRelativeToParent
        {
            get; set;
        }

        internal bool ParticleRandomAngle
        {
            get; set;
        }

        internal int ParticleMinRadius
        {
            get; set;
        }

        internal int ParticleMaxRadius
        {
            get; set;
        }

        internal int ParticleMinSpeed
        {
            get; set;
        }

        internal int ParticleMaxSpeed
        {
            get; set;
        }

        internal int ParticleStartXyAreaRadius
        {
            get; set;
        }

        internal int ParticleStartX
        {
            get; set;
        }

        internal int ParticleStartZ
        {
            get; set;
        }

        internal int ParticleMinVelocityZ
        {
            get; set;
        }

        internal int ParticleMaxVelocityZ
        {
            get; set;
        }

        internal int ParticleGravity
        {
            get; set;
        }

        internal int ParticleMinTailLength
        {
            get; set;
        }

        internal int ParticleMaxTailLength
        {
            get; set;
        }

        internal string ParticleResource
        {
            get; set;
        }

        internal string ParticleExportName
        {
            get; set;
        }

        internal bool RotateToDirection
        {
            get; set;
        }

        internal bool LoopParticleClip
        {
            get; set;
        }

        internal int StartScale
        {
            get; set;
        }

        internal int EndScale
        {
            get; set;
        }

        internal int FadeInDuration
        {
            get; set;
        }

        internal int FadeOutDuration
        {
            get; set;
        }

        internal int Inertia
        {
            get; set;
        }

        internal string EnemyVersion
        {
            get; set;
        }

        internal bool NoBounce
        {
            get; set;
        }

        internal bool StopOnBounce
        {
            get; set;
        }

        internal int RandomScale
        {
            get; set;
        }

        internal bool NoLowEndOptimization
        {
            get; set;
        }

        internal int SortingOffset
        {
            get; set;
        }

        internal bool Shadow
        {
            get; set;
        }

        internal int AngularSpeed
        {
            get; set;
        }

        internal int ShadowMulR
        {
            get; set;
        }

        internal int ShadowMulG
        {
            get; set;
        }

        internal int ShadowMulB
        {
            get; set;
        }

        internal int ShadowMulA
        {
            get; set;
        }

        internal bool InverseSpeed
        {
            get; set;
        }

        internal bool Trail
        {
            get; set;
        }

        internal int TrailWidth
        {
            get; set;
        }

        internal int TrailMaxPoints
        {
            get; set;
        }

        internal int TrailDuration
        {
            get; set;
        }

        internal string TrailSwf
        {
            get; set;
        }

        internal string TrailExportName
        {
            get; set;
        }

        internal string SpecialEffect
        {
            get; set;
        }

        internal bool FrameFromAngle
        {
            get; set;
        }

        internal int RotateMinSpeed
        {
            get; set;
        }

        internal int RotateMaxSpeed
        {
            get; set;
        }

        internal bool IgnoreShadowFlip
        {
            get; set;
        }

        internal bool ResourceFromAngle
        {
            get; set;
        }

    }
}