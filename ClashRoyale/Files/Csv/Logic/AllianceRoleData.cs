namespace ClashRoyale.Files.Csv.Logic
{
    public class AllianceRoleData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AllianceRoleData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AllianceRoleData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AllianceRoleData.
        }

        public int Level { get; set; }

        public string TID { get; set; }

        public bool CanInvite { get; set; }

        public bool CanSendMail { get; set; }

        public bool CanChangeAllianceSettings { get; set; }

        public bool CanAcceptJoinRequest { get; set; }

        public bool CanKick { get; set; }

        public bool CanBePromotedToLeader { get; set; }

        public bool CanPromoteToOwnLevel { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}