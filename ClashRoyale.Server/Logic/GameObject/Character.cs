namespace ClashRoyale.Server.Logic.GameObject
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Files.Csv;

    internal class Character : GameObject
    {
        internal int Level;

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        public Character(CsvData CsvData) : base(CsvData)
        {
            // Character.
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this.Level);

            base.Encode(Stream);
        }
    }
}