namespace ClashRoyale.Files.Csv.Logic
{
    public class TutorialData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TutorialData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TutorialData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TutorialNpcData.
        }

        public string Location { get; set; }

        public string Npc { get; set; }

        public string Tid { get; set; }

        public string ButtonTid { get; set; }

        public string FinishRequirement { get; set; }

        public string Chest { get; set; }

        public int WaitTimeMs { get; set; }

        public string FileName { get; set; }

        public int PopupCorner { get; set; }

        public string PopupExportName { get; set; }

        public string BubbleExportName { get; set; }

        public string Sound { get; set; }

        public bool Darkening { get; set; }

        public string BubbleObject { get; set; }

        public string OverlayExportName { get; set; }

        public string SpellDragExportName { get; set; }

        public string SpellToCast { get; set; }

        public bool ForceSpellTile { get; set; }

        public bool DisableOtherSpells { get; set; }

        public int SpellTileX { get; set; }

        public int SpellTileY { get; set; }

        public bool DisableSpells { get; set; }

        public bool HideCombatUi { get; set; }

        public bool DisableTroopMovement { get; set; }

        public bool DisableLeaderMovement { get; set; }

        public bool DisableSpawnPoints { get; set; }

        public bool DisableOpponentSpells { get; set; }

        public bool PauseCombat { get; set; }

        public string Dependency { get; set; }

        public int Priority { get; set; }

        public string Taunt { get; set; }

        public bool HighlightTargetsOnManaFull { get; set; }

        public bool DisableBattleStartScreen { get; set; }

        public int NpcMatchesPlayed { get; set; }

        public bool DisableBattleMenu { get; set; }

        public int CloseAutomaticallyAfterSeconds { get; set; }

        public int GroupMod { get; set; }

        public int GroupValue { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}