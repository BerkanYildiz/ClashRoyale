namespace ClashRoyale.Files.Sc
{
    using System.IO;

    public class ScStats
    {
        public short AnimationsCount;
        public short ColorTransformsCount;
        public short SheetsCount;
        public short SpiritesCount;
        public short StringsCount;
        public short TextsFieldCount;
        public short TransformsCount;

        /// <summary>
        ///     Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(BinaryReader Stream)
        {
            this.SpiritesCount = Stream.ReadInt16();
            this.AnimationsCount = Stream.ReadInt16();
            this.SheetsCount = Stream.ReadInt16();
            this.TextsFieldCount = Stream.ReadInt16();
            this.TransformsCount = Stream.ReadInt16();
            this.ColorTransformsCount = Stream.ReadInt16();

            for (int i = 0; i < 5; i++)
            {
                Stream.ReadByte();
            }

            this.StringsCount = Stream.ReadInt16();
        }

        /// <summary>
        ///     Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(BinaryWriter Stream)
        {
            Stream.Write(this.SpiritesCount);
            Stream.Write(this.AnimationsCount);
            Stream.Write(this.SheetsCount);
            Stream.Write(this.TextsFieldCount);
            Stream.Write(this.TransformsCount);
            Stream.Write(this.ColorTransformsCount);

            for (int i = 0; i < 5; i++)
            {
                Stream.Write((byte) 0x00);
            }

            Stream.Write(this.StringsCount);
        }

        /// <summary>
        ///     Logs this instance.
        /// </summary>
        public void Log()
        {
            Logging.Info(this.GetType(), "------------------------------------------------------------");
            Logging.Info(this.GetType(), "We've detected " + this.SpiritesCount + " spirites.");
            Logging.Info(this.GetType(), "We've detected " + this.AnimationsCount + " animations.");
            Logging.Info(this.GetType(), "We've detected " + this.SheetsCount + " sheets.");
            Logging.Info(this.GetType(), "We've detected " + this.TextsFieldCount + " text fields.");
            Logging.Info(this.GetType(), "We've detected " + this.TransformsCount + " transforms.");
            Logging.Info(this.GetType(), "We've detected " + this.ColorTransformsCount + " colortransforms.");
            Logging.Info(this.GetType(), "We've detected " + this.StringsCount + " strings.");
            Logging.Info(this.GetType(), "------------------------------------------------------------");
        }
    }
}