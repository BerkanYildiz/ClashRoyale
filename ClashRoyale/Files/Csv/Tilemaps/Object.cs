namespace ClashRoyale.Files.Csv.Tilemaps
{
    using System;
    using ClashRoyale.Files.Csv.Logic;

    public struct Object
    {
        public int X;
        public int Y;
        public string Name;
        public CharacterData Data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Object" /> struct.
        /// </summary>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        /// <param name="Name">The name.</param>
        /// <exception cref="System.Exception">Object == null at Object(X, Y, Name).</exception>
        public Object(int X, int Y, string Name)
        {
            this.X = X;
            this.Y = Y;
            this.Name = Name;
            this.Data = CsvFiles.Characters.Find(T => T.Name == Name);

            if (this.Data == null)
            {
                throw new Exception("Object == null at Object(X, Y, Name).");
            }
        }
    }
}