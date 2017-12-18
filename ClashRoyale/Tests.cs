namespace ClashRoyale
{
    using System;

    using ClashRoyale.Extensions;

    public static class Tests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            // Initialize.
        }

        /// <summary>
        /// Turns an integer to a VInt and dumps the result.
        /// </summary>
        /// <param name="Integer">The integer.</param>
        public static void IntToVInt(int Integer)
        {
            byte[] Buffer;

            using (ByteStream Stream = new ByteStream(5))
            {
                Stream.WriteVInt(Integer);
                Buffer = Stream.ToArray();
            }

            Logging.Warning(typeof(Tests), "Int TO VInt : " + BitConverter.ToString(Buffer));
        }
    }
}
