namespace ClashRoyale.Server.Files.Csv
{
    using Newtonsoft.Json;

    [JsonConverter(typeof(CsvConverter))]
    internal class CsvData
    {
        internal readonly int Type;
        internal readonly int Instance;
        internal readonly int GlobalId;

        internal readonly string Name;

        internal CsvRow CsvRow;
        internal CsvTable CsvTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvData"/> class.
        /// </summary>
        internal CsvData()
        {
            // CsvData.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvData"/> class.
        /// </summary>
        /// <param name="Row">The row.</param>
        /// <param name="Table">The table.</param>
        internal CsvData(CsvRow Row, CsvTable Table)
        {
            this.CsvRow     = Row;
            this.Name       = Row.Name;

            this.CsvTable   = Table;
            this.Type       = Table.Offset;
            this.Instance   = Table.Datas.Count;
            this.GlobalId   = Table.Datas.Count + 1000000 * Table.Offset;

            Row.LoadData(this);
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        internal virtual void LoadingFinished()
        {
            // LoadingFinished.
        }
    }
}