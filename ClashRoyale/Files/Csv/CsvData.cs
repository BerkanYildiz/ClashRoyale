namespace ClashRoyale.Files.Csv
{
    using Newtonsoft.Json;

    [JsonConverter(typeof(CsvConverter))]
    public class CsvData
    {
        public readonly int GlobalId;
        public readonly int Instance;

        public readonly string Name;
        public readonly int Type;

        public CsvRow CsvRow;
        public CsvTable CsvTable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvData" /> class.
        /// </summary>
        public CsvData()
        {
            // CsvData.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvData" /> class.
        /// </summary>
        /// <param name="Row">The row.</param>
        /// <param name="Table">The table.</param>
        public CsvData(CsvRow Row, CsvTable Table)
        {
            this.CsvRow = Row;
            this.Name = Row.Name;

            this.CsvTable = Table;
            this.Type = Table.Offset;
            this.Instance = Table.Datas.Count;
            this.GlobalId = Table.Datas.Count + 1000000 * Table.Offset;

            Row.LoadData(this);
        }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public virtual void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}