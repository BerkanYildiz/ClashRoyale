namespace ClashRoyale.Files.Csv.Tilemaps
{
    using System.Collections.Generic;

    public class MapRow
    {
        public string Name;

        public List<string> Columns;
        public List<string> ValueType;
        public List<string[]> Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRow"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        public MapRow(string Name)
        {
            this.Name       = Name;

            this.Columns    = new List<string>();
            this.ValueType  = new List<string>();
            this.Values     = new List<string[]>();
        }
    }
}
