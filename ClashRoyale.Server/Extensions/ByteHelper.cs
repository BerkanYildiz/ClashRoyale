namespace ClashRoyale.Server.Extensions
{
    using System;
    using System.Linq;

    internal static class ByteHelper
    {
        /// <summary>
        /// Gets the bytes at the specified offset to the specified length.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        /// <param name="Offset">The offset.</param>
        /// <param name="Length">The length.</param>
        /// <param name="Content">The content.</param>
        internal static bool Get(this byte[] Buffer, int Offset, int Length, out byte[] Content)
        {
            if (Buffer.Length - Offset >= Length)
            {
                Content = new byte[Length];
                Array.Copy(Buffer, Offset, Content, 0, Length);

                return true;
            }

            Content = null;

            return false;
        }

        /// <summary>
        /// Gets the bytes at the specified offset to the specified length.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        /// <param name="Offset">The offset.</param>
        /// <param name="Length">The length.</param>
        internal static byte[] Get(this byte[] Buffer, int Offset, int Length)
        {
            if (Buffer.Length - Offset >= Length)
            {
                byte[] Content = new byte[Length];
                Array.Copy(Buffer, Offset, Content, 0, Length);
                return Content;
            }

            return null;
        }

        /// <summary>
        /// Turn a hexa string into a byte array.
        /// </summary>
        /// <param name="HexaString">The hexa string.</param>
        public static byte[] HexaToBytes(this string HexaString)
        {
            string TrimmedHexa = HexaString.Replace("-", string.Empty).Replace(" ", string.Empty);
            return Enumerable.Range(0, TrimmedHexa.Length).Where(X => X % 2 == 0) .Select(X => Convert.ToByte(TrimmedHexa.Substring(X, 2), 16)).ToArray();
        }
    }
}