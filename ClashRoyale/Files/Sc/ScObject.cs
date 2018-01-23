namespace ClashRoyale.Files.Sc
{
    using System.IO;

    public class ScObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScObject" /> class.
        /// </summary>
        public ScObject()
        {
            // ScObject.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScObject" /> class.
        /// </summary>
        /// <param name="BlockType">Type of the block.</param>
        public ScObject(short BlockType)
        {
            this.Type = BlockType;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScObject" /> class.
        /// </summary>
        /// <param name="ScFile">The sc file.</param>
        /// <param name="BlockType">Type of the block.</param>
        public ScObject(ScFile ScFile, short BlockType)
        {
            this.ScFile = ScFile;
            this.Type = BlockType;
        }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public short Type { get; }

        /// <summary>
        ///     Gets the SC file.
        /// </summary>
        public ScFile ScFile { get; }

        /// <summary>
        ///     Decodes this instance from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public virtual void Decode(BinaryReader Stream)
        {
            // Decode.
        }

        /// <summary>
        ///     Encodes this instance in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public virtual void Encode(BinaryWriter Stream)
        {
            // Encode.
        }
    }
}