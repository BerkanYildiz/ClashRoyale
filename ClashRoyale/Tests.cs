namespace ClashRoyale
{
    using System;

    using ClashRoyale.Compression.ZLib;
    using ClashRoyale.Extensions;

    public static class Tests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            // Initialize.
            // Decompress("78-9C-95-51-CF-6B-13-51-10-9E-6F-F6-CD-CB-C6-34-A9-42-7F-D0-67-5B-5A-62-63-A9-90-83-87-1E-0B-D5-F4-10-50-41-8D-D0-5B-10-EC-A1-28-56-AC-5E-A4-76-15-2F-41-34-7A-51-AB-C4-5E-0C-25-05-51-B1-20-EA-45-2F-FE-03-42-8F-22-78-F2-DC-B3-50-67-7F-24-E6-A0-42-BF-E5-CD-BE-FD-E6-9B-99-9D-19-4C-31-83-0D-48-31-54-29-97-AA-C7-67-CF-56-AA-A7-CF-CD-A9-3D-59-3E-55-9D-3B-51-9E-2F-9F-09-BD-A3-A1-21-4B-00-19-F3-57-F9-EC-7C-97-FC-68-24-27-56-39-02-45-FC-89-84-4E-E0-B7-2F-DC-45-82-F9-D5-77-B4-4F-58-F3-D8-C2-D5-8B-E7-2F-EF-7B-3E-B1-66-D6-0E-45-9A-42-1C-9E-13-DC-F9-00-E1-B7-46-3C-12-43-D2-F3-22-27-59-92-1C-49-EF-03-2B-03-0F-AD-0C-BD-F9-B6-BE-03-71-69-19-26-19-D9-AA-6D-97-24-1F-26-15-B1-9F-26-25-F5-88-25-53-90-3E-5F-0E-A6-7D-37-78-17-6E-C4-B8-7C-C1-4D-1C-70-53-FD-AE-68-DC-B4-0C-67-66-88-D2-3C-FA-E4-FE-A2-86-65-2E-2D-2D-8F-5D-59-BC-76-FD-C2-D2-F2-16-9E-E6-1A-4C-9B-F6-A3-E0-1D-BC-F0-D7-8F-D0-78-90-A1-CA-EA-7A-E9-E7-C6-D7-4D-B0-AF-BD-53-D2-46-0C-AB-D7-74-28-D5-53-E7-9D-9B-CA-DC-B0-79-24-0F-E5-D5-A1-2E-08-89-F6-25-9E-18-11-3F-DB-FC-81-46-5D-0B-04-F4-79-A5-33-2C-64-5F-4E-37-17-42-F2-96-89-F3-85-23-51-B2-4B-8A-B6-54-33-FC-D1-76-12-B4-EE-E1-7D-B1-CD-C6-70-C6-C3-0A-0C-5B-49-51-B8-36-5D-9C-44-8E-20-41-5C-45-23-6B-5F-3A-65-DA-A1-12-AC-1A-83-14-79-FF-0E-DD-2B-36-0E-53-6B-30-B1-B5-49-6A-CD-44-6C-13-FF-B7-BB-00-18-1E-FA-60-E8-36-A8-0E-EC-27-DA-C5-98-2D-62-40-1C-1E-C3-7F-06-1E-A7-7E-90-58-66-C3-D1-F6-62-E8-70-7A-F4-D5-D8-7E-FD-0B-F4-1B-97-1D-77-45");
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
    }
}
