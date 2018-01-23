namespace ClashRoyale.Files.Csv.Logic
{
    using ClashRoyale.Enums;

    public class ResourcePackData : CsvData
    {
        public ResourceData ResourceData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourcePackData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ResourcePackData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ResourcePackData.
        }

        public string TID { get; set; }

        public string Resource { get; set; }

        public int Amount { get; set; }

        public string IconFile { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            this.ResourceData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>(this.Resource);

            if (this.ResourceData == null)
            {
                Logging.Error(this.GetType(), "Resource " + this.Resource + " not exist.");
            }
        }
    }
}