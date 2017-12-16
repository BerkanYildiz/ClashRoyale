namespace ClashRoyale.Server.Files.Csv.Logic.Quests
{
    internal class AttackData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttackData"/> class.
        /// </summary>
        /// <param name="CsvRow"></param>
        /// <param name="CsvTable"></param>
        public AttackData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AttackData.
        }
	
        internal int Size
        {
            get; set;
        }

        internal string Title
        {
            get; set;
        }

        internal string Info
        {
            get; set;
        }

        internal string ItemFile
        {
            get; set;
        }

        internal string ItemExportName
        {
            get; set;
        }

        internal int Count
        {
            get; set;
        }

        internal string GameType
        {
            get; set;
        }

        internal string PlayerType
        {
            get; set;
        }

        internal string Spell
        {
            get; set;
        }

        internal string Character
        {
            get; set;
        }

        internal string TargetCharacter
        {
            get; set;
        }

        internal int Weight
        {
            get; set;
        }

        internal string Type
        {
            get; set;
        }

        internal string MinArena
        {
            get; set;
        }

        internal string MaxArena
        {
            get; set;
        }

    }
}