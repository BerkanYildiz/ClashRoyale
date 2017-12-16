namespace ClashRoyale.Server.Files.Csv
{
    using System.Collections.Generic;

    internal class CsvTable
    {
        internal readonly List<CsvData> Datas;

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

            this.Datas  = new List<CsvData>();
            var Reader  = new CsvReader(Path);

            for (int i = 0; i < Reader.GetRowCount(); i++)
            {
                CsvRow Row      = Reader.GetRowAt(i);
                CsvData Data    = this.Load(Row);

                if (Data != null)
                {
                    this.Datas.Add(Data);
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
            foreach (CsvData CsvData in this.Datas)
            {
                CsvData.LoadingFinished();
            }
        }

        /// <summary>
        /// Gets the data with identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        internal CsvData GetWithInstanceId(int Identifier)
        {
            if (this.Datas.Count > Identifier)
            {
                return this.Datas[Identifier];
            }

            return null;
        }

        /// <summary>
        /// Gets the data with identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        internal T GetWithInstanceId<T>(int Identifier) where T : CsvData
        {
            if (this.Datas.Count > Identifier)
            {
                return this.Datas[Identifier] as T;
            }

            return null;
        }

        /// <summary>
        /// Gets the data with identifier.
        /// </summary>
        /// <param name="GlobalId">The identifier.</param>
        internal CsvData GetWithGlobalId(int GlobalId)
        {
            return this.GetWithInstanceId(GlobalId % 1000000);
        }

        /// <summary>
        /// Gets the data with identifier.
        /// </summary>
        /// <param name="GlobalId">The identifier.</param>
        internal T GetWithGlobalId<T>(int GlobalId) where T : CsvData
        {
            return this.GetWithInstanceId(GlobalId % 1000000) as T;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="Name">The name.</param>
        internal CsvData GetData(string Name)
        {
            return this.Datas.Find(Data => Data.Name == Name);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="Name">The name.</param>
        internal T GetData<T>(string Name) where T : CsvData
        {
            return this.Datas.Find(Data => Data.Name == Name) as T;
        }
    }
}