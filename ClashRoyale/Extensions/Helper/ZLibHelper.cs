namespace ClashRoyale.Extensions.Helper
{
    using System;

    using ClashRoyale.Compression.ZLib;

    public static class ZLibHelper
    {
        /// <summary>
        /// Compresses the specified byte array.
        /// </summary>
        internal static byte[] CompressByteArray(byte[] Input)
        {
            byte[] ZLibOutput = ZlibStream.CompressBuffer(Input);
            byte[] Output = new byte[ZLibOutput.Length + 4];

            Output[0] = (byte) (Input.Length);
            Output[1] = (byte) (Input.Length >> 8);
            Output[2] = (byte) (Input.Length >> 16);
            Output[3] = (byte) (Input.Length >> 24);

            Array.Copy(ZLibOutput, 0, Output, 4, ZLibOutput.Length);

            return Output;
        }

        public static byte[] CompressCompressableByteArray(byte[] Input)
        {
            if (Input.Length > 100)
            {
                byte[] Compressed = ZLibHelper.CompressByteArray(Input);
                byte[] Output = new byte[Compressed.Length + 1];

                Output[0] = 1;

                Array.Copy(Compressed, 0, Output, 1, Compressed.Length);

                return Output;
            }

            return Input;
        }

        public static byte[] DecompressCompressableByteArray(byte[] Input)
        {
            return ZlibStream.UncompressBuffer(Input);
        }
    }
}