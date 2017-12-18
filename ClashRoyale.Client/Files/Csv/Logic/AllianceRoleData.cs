namespace ClashRoyale.Client.Files.Csv.Logic
{
    internal class AllianceRoleData : CsvData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="AllianceRoleData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AllianceRoleData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AllianceRoleData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
	    	// LoadingFinished.
		}
	
        internal int Level
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

        internal bool CanInvite
        {
            get; set;
        }

        internal bool CanSendMail
        {
            get; set;
        }

        internal bool CanChangeAllianceSettings
        {
            get; set;
        }

        internal bool CanAcceptJoinRequest
        {
            get; set;
        }

        internal bool CanKick
        {
            get; set;
        }

        internal bool CanBePromotedToLeader
        {
            get; set;
        }

        internal bool CanPromoteToOwnLevel
        {
            get; set;
        }

    }
}