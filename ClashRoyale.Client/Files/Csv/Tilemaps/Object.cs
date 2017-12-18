namespace ClashRoyale.Client.Files.Csv.Tilemaps
{
    using System;

    using ClashRoyale.Client.Files.Csv.Logic;

    internal struct Object
    {
        internal int X;
        internal int Y;
        internal string Name;
        internal CharacterData Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/> struct.
        /// </summary>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        /// <param name="Name">The name.</param>
        /// <exception cref="System.Exception">Object == null at Object(X, Y, Name).</exception>
        internal Object(int X, int Y, string Name)
        {
            this.X      = X;
            this.Y      = Y;
            this.Name   = Name;
            this.Data   = CsvFiles.Characters.Find(T => T.Name == Name);

            if (this.Data == null)
            {
                throw new Exception("Object == null at Object(X, Y, Name).");
            }
        }
    }
}
