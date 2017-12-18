namespace ClashRoyale.Files.Csv.Logic
{
    public class AbilityData : CsvData
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
		public override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        public string IconFile
        {
            get; set;
        }

        public string Tid
        {
            get; set;
        }

        public string AreaEffectObject
        {
            get; set;
        }

        public string Buff
        {
            get; set;
        }

        public int BuffTime
        {
            get; set;
        }

        public string Effect
        {
            get; set;
        }

    }
}