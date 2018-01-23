namespace ClashRoyale.Files.Csv
{
    using System.Collections.Generic;
    using Microsoft.VisualBasic.FileIO;

    public class CsvReader
    {
        private readonly List<Column> Columns;

        private readonly List<string> Headers;
        private readonly List<CsvRow> Rows;
        private readonly List<string> Types;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvReader" /> class.
        /// </summary>
        /// <param name="Path">The path.</param>
        public CsvReader(string Path)
        {
            this.Columns = new List<Column>();
            this.Rows = new List<CsvRow>();

            this.Headers = new List<string>();
            this.Types = new List<string>();

            using (TextFieldParser Reader = new TextFieldParser(Path))
            {
                Reader.SetDelimiters(",");

                string[] Columns = Reader.ReadFields();

                foreach (string Column in Columns)
                {
                    this.Headers.Add(Column);
                    this.Columns.Add(new Column());
                }

                string[] Types = Reader.ReadFields();

                foreach (string Type in Types)
                {
                    this.Types.Add(Type);
                }

                while (!Reader.EndOfData)
                {
                    string[] Values = Reader.ReadFields();

                    if (!string.IsNullOrEmpty(Values[0]))
                    {
                        this.AddRow(new CsvRow(this));
                    }

                    for (int i = 0; i < this.Headers.Count; i++)
                    {
                        this.Columns[i].Add(Values[i]);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the row at.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        /// <returns></returns>
        public CsvRow GetRowAt(int Offset)
        {
            return this.Rows[Offset];
        }

        /// <summary>
        ///     Gets the row count.
        /// </summary>
        /// <returns></returns>
        public int GetRowCount()
        {
            return this.Rows.Count;
        }

        /// <summary>
        ///     Gets the value at the specified offset.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Offset">The offset.</param>
        public string GetValue(string Name, int Offset)
        {
            int RowIndex = this.Headers.IndexOf(Name);
            return this.GetValueAt(RowIndex, Offset);
        }

        /// <summary>
        ///     Gets the value at the specified offsets.
        /// </summary>
        /// <param name="Column">The column.</param>
        /// <param name="Row">The row.</param>
        public string GetValueAt(int Column, int Row)
        {
            if (Column > -1 && Row > -1)
            {
                return this.Columns[Column].Get(Row);
            }

            return null;
        }

        /// <summary>
        ///     Adds the specified row.
        /// </summary>
        /// <param name="Row">The row.</param>
        public void AddRow(CsvRow Row)
        {
            this.Rows.Add(Row);
        }

        /// <summary>
        ///     Gets the array size.
        /// </summary>
        /// <param name="Row">The row.</param>
        /// <param name="ColumnIndex">Index of the column.</param>
        public int GetArraySizeAt(CsvRow Row, int ColumnIndex)
        {
            int i = this.Rows.IndexOf(Row);

            if (i == -1)
            {
                return 0;
            }

            Column Column = this.Columns[ColumnIndex];

            int NextOffset;

            if (i + 1 >= this.Rows.Count)
            {
                NextOffset = Column.GetSize();
            }
            else
            {
                NextOffset = this.Rows[i + 1].Offset;
            }

            int Offset = Row.Offset;

            return Column.GetSize(Offset, NextOffset);
        }

        /// <summary>
        ///     Gets the index of the column using its name.
        /// </summary>
        /// <param name="Name">The name.</param>
        public int GetColumnIndexByName(string Name)
        {
            return this.Headers.IndexOf(Name);
        }

        /// <summary>
        ///     Gets the name of the column at the specified offset.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        public string GetColumnName(int Offset)
        {
            return this.Headers[Offset];
        }

        /// <summary>
        ///     Gets the column row count.
        /// </summary>
        public int GetColumnRowCount()
        {
            if (this.Columns.Count > 0)
            {
                return this.Columns[0].GetSize();
            }

            return 0;
        }
    }
}