namespace ClashRoyale.Extensions
{
    using System;
    using System.IO;
    using System.Text;

    public static class BinaryWriterHelper
    {
        /// <summary>
        /// Writes the ASCII, using the <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        /// <param name="Value">The value to write.</param>
        public static void WriteAscii(this BinaryWriter Stream, string Value)
        {
            if (string.IsNullOrEmpty(Value))
            {
                Stream.Write(0xFF);
            }
            else
            {
                if (Value.Length > 255)
                {
                    throw new Exception("String length inferior to 256 characters expected.");
                }

                Stream.Write((byte) Value.Length);
                Stream.Write(Encoding.UTF8.GetBytes(Value));
            }
        }
    }
}
