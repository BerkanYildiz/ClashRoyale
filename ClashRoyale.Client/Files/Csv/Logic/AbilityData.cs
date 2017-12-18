namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class AbilityData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="AbilityData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AbilityData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AbilityData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal string IconFile
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

        internal string AreaEffectObject
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

        internal string Effect
        {
            get; set;
        }

    }
}