namespace ClashRoyale.Logic.GameObject
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv;

    public class Character : GameObject
    {
        public int Level;

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        public Character(CsvData CsvData) : base(CsvData)
        {
            // Characters.
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this.Level);

            base.Encode(Stream);
        }
    }
}