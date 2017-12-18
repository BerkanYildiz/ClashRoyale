namespace ClashRoyale.Client.Files.Csv.Tilemaps
{
    using System.Collections.Generic;

    internal class MapRow
    {
        internal string Name;

        internal List<string> Columns;
        internal List<string> ValueType;
        internal List<string[]> Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRow"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        internal MapRow(string Name)
        {
            this.Name       = Name;

            this.Columns    = new List<string>();
            this.ValueType  = new List<string>();
            this.Values     = new List<string[]>();
        }
    }
}
