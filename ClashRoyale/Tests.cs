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

            Logging.Warning(typeof(Tests), "Int TO VInt : " + BitConverter.ToString(Buffer) + ".");
        }

        /// <summary>
        /// Turns a VInt to an integer and dumps the result.
        /// </summary>
        /// <param name="VInt">The VInt in hexa.</param>
        public static void VIntToInt(string VInt)
        {
            int Integer;

            using (ByteStream Stream = new ByteStream(VInt.HexaToBytes()))
            {
                Integer = Stream.ReadVInt();
            }

            Logging.Warning(typeof(Tests), "VInt TO Int : " + Integer + ".");
        }

        /// <summary>
        /// Test the writer for the compressed booleans.
        /// </summary>
        public static void Booleans()
        {
            byte[] Buffer;

            using (ByteStream Stream = new ByteStream(1))
            {
                for (int I = 0; I < 8; I++)
                {
                    Stream.WriteBoolean(true);
                }

                Buffer = Stream.ToArray();
            }

            Logging.Info(typeof(Tests), "Booleans : " + BitConverter.ToString(Buffer) + ".");
        }
        
        /// <summary>
        /// Test the writer for the compressed booleans.
        /// </summary>
        public static void Booleans(params bool[] Values)
        {
            byte[] Buffer;

            using (ByteStream Stream = new ByteStream(Values.Length))
            {
                for (int I = 0; I < Values.Length; I++)
                {
                    Stream.WriteBoolean(Values[I]);
                }

                Buffer = Stream.ToArray();
            }

            Logging.Info(typeof(Tests), "Booleans : " + BitConverter.ToString(Buffer) + ".");
        }
    }
}
