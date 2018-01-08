namespace ClashRoyale
{
    using System;

    using ClashRoyale.Api;
    using ClashRoyale.Compression.ZLib;
    using ClashRoyale.Extensions;

    public static class Tests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            // Tests.
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

            Logging.Info(typeof(Tests), "Int TO VInt : " + BitConverter.ToString(Buffer) + ".");
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

            Logging.Info(typeof(Tests), "VInt TO Int : " + Integer + ".");
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

        /// <summary>
        /// Compresses the specified hexa.
        /// </summary>
        public static void Compress(string Hexa)
        {
            byte[] Decompressed = Hexa.HexaToBytes();
            byte[] Compressed   = ZlibStream.CompressBuffer(Decompressed);

            Logging.Info(typeof(Tests), "Compressed : " + BitConverter.ToString(Compressed));
        }
        
        /// <summary>
        /// Decompresses the specified hexa.
        /// </summary>
        public static void Decompress(string Hexa)
        {
            byte[] Compressed   = Hexa.HexaToBytes();
            byte[] Decompressed = ZlibStream.UncompressBuffer(Compressed);

            Logging.Info(typeof(Tests), "Decompressed : " + BitConverter.ToString(Decompressed));
        }

        /// <summary>
        /// Gets informations about the specified ip address.
        /// </summary>
        /// <param name="IpAddress">The ip address.</param>
        public static void GetIp(string IpAddress)
        {
            var Result = IpRequester.GetIpInfo(IpAddress).Result;

            if (Result.IsSuccess)
            {
                Logging.Info(typeof(Tests), "IP Address : " + IpAddress);
                Logging.Info(typeof(Tests), "Result :");
                Logging.Info(typeof(Tests), "    - City      " + Result.City + ".");
                Logging.Info(typeof(Tests), "    - Region    " + Result.Region + ".");
                Logging.Info(typeof(Tests), "    - Country   " + Result.Country + ".");
            }
            else
            {
                Logging.Info(typeof(Tests), "Failed to retrieve data about " + IpAddress + ".");
            }
        }
    }
}
