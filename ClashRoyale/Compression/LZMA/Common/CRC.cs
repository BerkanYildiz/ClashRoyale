namespace ClashRoyale.Compression.LZMA.Common
{
    internal class Crc
    {
        public static readonly uint[] Table;

        private uint Value = 0xFFFFFFFF;

        static Crc()
        {
            Crc.Table = new uint[256];
            const uint KPoly = 0xEDB88320;
            for (uint i = 0; i < 256; i++)
            {
                uint r = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((r & 1) != 0)
                    {
                        r = (r >> 1) ^ KPoly;
                    }
                    else
                    {
                        r >>= 1;
                    }
                }

                Crc.Table[i] = r;
            }
        }

        public uint GetDigest()
        {
            return this.Value ^ 0xFFFFFFFF;
        }

        public void Init()
        {
            this.Value = 0xFFFFFFFF;
        }

        public void Update(byte[] Data, uint Offset, uint Size)
        {
            for (uint i = 0; i < Size; i++)
            {
                this.Value = Crc.Table[(byte)this.Value ^ Data[Offset + i]] ^ (this.Value >> 8);
            }
        }

        public void UpdateByte(byte B)
        {
            this.Value = Crc.Table[(byte)this.Value ^ B] ^ (this.Value >> 8);
        }

        private static uint CalculateDigest(byte[] Data, uint Offset, uint Size)
        {
            Crc crc = new Crc();

            // crc.Init();
            crc.Update(Data, Offset, Size);
            return crc.GetDigest();
        }

        private static bool VerifyDigest(uint Digest, byte[] Data, uint Offset, uint Size)
        {
            return Crc.CalculateDigest(Data, Offset, Size) == Digest;
        }
    }
}