namespace ClashRoyale.Files.Csv.Client
{
    public class TextData : CsvData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TextData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TextData.
        }

        public string EN { get; set; }

        public string FR { get; set; }

        public string DE { get; set; }

        public string ES { get; set; }

        public string IT { get; set; }

        public string NL { get; set; }

        public string NO { get; set; }

        public string TR { get; set; }

        public string JP { get; set; }

        public string KR { get; set; }

        public string RU { get; set; }

        public string AR { get; set; }

        public string PT { get; set; }

        public string CN { get; set; }

        public string CNT { get; set; }

        public string FA { get; set; }

        public string ID { get; set; }

        public string MS { get; set; }

        public string TH { get; set; }

        public string FI { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}