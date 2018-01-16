namespace ClashRoyale.Files.Csv.Logic
{
    using ClashRoyale.Extensions.Game;

    public class ResourcePackData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ResourcePackData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ResourcePackData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ResourcePackData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		public override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        public string Tid
        {
            get; set;
        }

        public string Resource
        {
            get; set;
        }

        public int Amount
        {
            get; set;
        }

        public string IconFile
        {
            get; set;
        }

        /// <summary>
        /// Gets the cost.
        /// </summary>
        public int Cost
        {
            get
            {
                if (this.Amount > 0)
                {
                    if (this.Amount > 9)
                    {
                        if (this.Amount > 99)
                        {
                            if (this.Amount > 999)
                            {
                                if (this.Amount > 9999)
                                {
                                    if (this.Amount > 99999)
                                    {
                                        return Globals.ResourceDiamondCost100000 + ((Globals.ResourceDiamondCost1000000 - Globals.ResourceDiamondCost100000) * (this.Amount / 100 - 1000) + 4500) / 9000;
                                    }

                                    return Globals.ResourceDiamondCost10000 + ((Globals.ResourceDiamondCost100000 - Globals.ResourceDiamondCost10000) * (this.Amount / 10 - 1000) + 4500) / 9000;
                                }

                                return Globals.ResourceDiamondCost1000 + ((Globals.ResourceDiamondCost10000 - Globals.ResourceDiamondCost1000) * (this.Amount - 1000) + 4500) / 9000;
                            }

                            return Globals.ResourceDiamondCost100 + ((Globals.ResourceDiamondCost1000 - Globals.ResourceDiamondCost100) * (this.Amount - 100) + 4500) / 9000;
                        }

                        return Globals.ResourceDiamondCost10 + ((Globals.ResourceDiamondCost100 - Globals.ResourceDiamondCost10) * (this.Amount - 10) + 4500) / 9000;
                    }

                    return Globals.ResourceDiamondCost1;
                }

                return 0;
            }
        }
    }
}