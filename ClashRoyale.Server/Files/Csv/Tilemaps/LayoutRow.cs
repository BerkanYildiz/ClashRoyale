namespace ClashRoyale.Server.Files.Csv.Tilemaps
{
    using System.Collections.Generic;

    internal class LayoutRow
    {
        internal string Name;

        internal List<string> Columns;
        internal List<string> ValueType;
        internal List<string[]> Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRow"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        internal LayoutRow(string Name)
        {
            this.Name       = Name;

            this.Columns    = new List<string>();
            this.ValueType  = new List<string>();
            this.Values     = new List<string[]>();
        }

        /// <summary>
        /// Gets the value at the specified index and name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Index">The index.</param>
        internal string GetValueAt(string Name, int Index)
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
