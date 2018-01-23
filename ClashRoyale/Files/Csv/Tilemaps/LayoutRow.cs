namespace ClashRoyale.Files.Csv.Tilemaps
{
    using System.Collections.Generic;

    public class LayoutRow
    {
        public List<string> Columns;
        public string Name;
        public List<string[]> Values;
        public List<string> ValueType;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LayoutRow" /> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        public LayoutRow(string Name)
        {
            this.Name = Name;

            this.Columns = new List<string>();
            this.ValueType = new List<string>();
            this.Values = new List<string[]>();
        }

        /// <summary>
        ///     Gets the value at the specified index and name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Index">The index.</param>
        public string GetValueAt(string Name, int Index)
        {
            int ColumnIdx = this.Columns.FindIndex(T => T == Name);

            if (ColumnIdx > -1)
            {
                return this.Values[Index][ColumnIdx];
            }

            return null;
        }
    }
}