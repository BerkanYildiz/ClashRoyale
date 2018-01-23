namespace ClashRoyale.Files.Csv
{
    using System.Collections.Generic;
    using System.Linq;

    public class Column
    {
        private readonly List<string> Values;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Column" /> class.
        /// </summary>
        public Column()
        {
            this.Values = new List<string>();
        }

        /// <summary>
        ///     Gets the size from a specified offset to the specified limit.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        /// <param name="Limit">The limit.</param>
        public static int GetSize(int Offset, int Limit)
        {
            return Limit - Offset;
        }

        /// <summary>
        ///     Adds the specified value.
        /// </summary>
        /// <param name="Value">The value.</param>
        public void Add(string Value)
        {
            if (Value == null)
            {
                Value = string.Empty;

                if (this.Values.Count > 0)
                {
                    Value = this.Values.Last();
                }
            }

            this.Values.Add(Value);
        }

        /// <summary>
        ///     Gets the column name at the specified index.
        /// </summary>
        /// <param name="ColumnIndex">The column index.</param>
        public string Get(int ColumnIndex)
        {
            if (this.Values.Count > ColumnIndex)
            {
                return this.Values[ColumnIndex];
            }

            return null;
        }

        /// <summary>
        ///     Gets the size.
        /// </summary>
        public int GetSize()
        {
            return this.Values.Count;
        }
    }
}