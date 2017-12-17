namespace ClashRoyale.Server
{
    using System;

    using ClashRoyale.Server.Extensions;

    internal static class Tests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            IntToVInt(156);
        }

        /// <summary>
        /// Turns an integer to a VInt and dumps the result.
        /// </summary>
        /// <param name="Integer">The integer.</param>
        internal static void IntToVInt(int Integer)
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
