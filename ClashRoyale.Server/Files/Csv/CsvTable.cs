namespace ClashRoyale.Server.Files.Csv
{
    using System.Collections.Generic;

    internal class CsvTable
    {
        internal readonly List<CsvData> Files;

        /// <summary>
        /// Gets the offset of this <see cref="CsvTable"/>.
        /// </summary>
        internal int Offset
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        /// <param name="Path">The path.</param>
        internal CsvTable(int Offset, string Path)
        {
            this.Offset = Offset;

            this.Files  = new List<CsvData>();
            var Reader  = new CsvReader(Path);

            for (int i = 0; i < Reader.GetRowCount(); i++)
            {
                CsvRow Row      = Reader.GetRowAt(i);
                CsvData Data    = this.Load(Row);

                if (Data != null)
                {
                    this.Files.Add(Data);
                }
            }
        }

        /// <summary>
        /// Loads the specified CSV row.
        /// </summary>
        /// <param name="CsvRow">The CSV row.</param>
        internal CsvData Load(CsvRow CsvRow)
        {
            return new CsvData(CsvRow, this);
        }

        /// <summary>
        /// Finishes every <see cref="CsvData"/> in this instance.
        /// </summary>
        internal void Finish()
        {
            foreach (CsvData CsvData in this.Files)
            {
                CsvData.LoadingFinished();
            }
        }
    }
}