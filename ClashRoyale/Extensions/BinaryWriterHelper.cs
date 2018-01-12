namespace ClashRoyale.Extensions
{
    using System.IO;
    using System.Text;

    public static class BinaryReaderHelper
    {
        /// <summary>
        /// Reads the ASCII, using the <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public static string ReadAscii(this BinaryReader Stream)
        {
            byte Length = (byte) Stream.Read();

            if (Length > 0)
            {
                if (Length == 0xFF)
                {
                    return null;
                }

                return Encoding.UTF8.GetString(Stream.ReadBytes(Length));
            }

            return string.Empty;
        }
    }
}
