namespace ClashRoyale.Compression.LZMA.Compress.RangeCoder
{
    using System.IO;

    internal class Encoder
    {
        public const uint kTopValue = 1 << 24;

        public ulong Low;

        public uint Range;

        private byte Cache;

        private uint CacheSize;

        private long StartPosition;

        private Stream Stream;

        public void CloseStream()
        {
            this.Stream.Close();
        }

        public void Encode(uint Start, uint Size, uint Total)
        {
            this.Low += Start * (this.Range /= Total);
            this.Range *= Size;
            while (this.Range < Encoder.kTopValue)
            {
                this.Range <<= 8;
                this.ShiftLow();
            }
        }

        public void EncodeBit(uint Size0, int NumTotalBits, uint Symbol)
        {
            uint NewBound = (this.Range >> NumTotalBits) * Size0;
            if (Symbol == 0)
            {
                this.Range = NewBound;
            }
            else
            {
                this.Low += NewBound;
                this.Range -= NewBound;
            }

            while (this.Range < Encoder.kTopValue)
            {
                this.Range <<= 8;
                this.ShiftLow();
            }
        }

        public void EncodeDirectBits(uint V, int NumTotalBits)
        {
            for (int i = NumTotalBits - 1; i >= 0; i--)
            {
                this.Range >>= 1;
                if (((V >> i) & 1) == 1)
                {
                    this.Low += this.Range;
                }

                if (this.Range < Encoder.kTopValue)
                {
                    this.Range <<= 8;
                    this.ShiftLow();
                }
            }
        }

        public void FlushData()
        {
            for (int i = 0; i < 5; i++)
            {
                this.ShiftLow();
            }
        }

        public void FlushStream()
        {
            this.Stream.Flush();
        }

        public long GetProcessedSizeAdd()
        {
            return this.CacheSize + this.Stream.Position - this.StartPosition + 4;

            // (long)Stream.GetProcessedSize();
        }

        public void Init()
        {
            this.StartPosition = this.Stream.Position;
            this.Low = 0;
            this.Range = 0xFFFFFFFF;
            this.CacheSize = 1;
            this.Cache = 0;
        }

        public void ReleaseStream()
        {
            this.Stream = null;
        }

        public void SetStream(Stream Stream)
        {
            this.Stream = Stream;
        }

        public void ShiftLow()
        {
            if ((uint)this.Low < 0xFF000000 || (uint)(this.Low >> 32) == 1)
            {
                byte temp = this.Cache;
                do
                {
                    this.Stream.WriteByte((byte)(temp + (this.Low >> 32)));
                    temp = 0xFF;
                }
                while (--this.CacheSize != 0);

                this.Cache = (byte)((uint)this.Low >> 24);
            }

            this.CacheSize++;
            this.Low = (uint)this.Low << 8;
        }
    }

    internal class Decoder
    {
        public const uint kTopValue = 1 << 24;

        public uint Code;

        public uint Range;

        // public Buffer.InBuffer Stream = new Buffer.InBuffer(1 << 16);
        public Stream Stream;

        public void CloseStream()
        {
            this.Stream.Close();
        }

        public void Decode(uint Start, uint Size, uint Total)
        {
            this.Code -= Start * this.Range;
            this.Range *= Size;
            this.Normalize();
        }

        public uint DecodeBit(uint Size0, int NumTotalBits)
        {
            uint NewBound = (this.Range >> NumTotalBits) * Size0;
            uint symbol;
            if (this.Code < NewBound)
            {
                symbol = 0;
                this.Range = NewBound;
            }
            else
            {
                symbol = 1;
                this.Code -= NewBound;
                this.Range -= NewBound;
            }

            this.Normalize();
            return symbol;
        }

        public uint DecodeDirectBits(int NumTotalBits)
        {
            uint range = this.Range;
            uint code = this.Code;
            uint result = 0;
            for (int i = NumTotalBits; i > 0; i--)
            {
                range >>= 1;
                /*
                                                result <<= 1;
                                                if (code >= range)
                                                {
                                                    code -= range;
                                                    result |= 1;
                                                }
                                                */
                uint t = (code - range) >> 31;
                code -= range & (t - 1);
                result = (result << 1) | (1 - t);
                if (range < Decoder.kTopValue)
                {
                    code = (code << 8) | (byte)this.Stream.ReadByte();
                    range <<= 8;
                }
            }

            this.Range = range;
            this.Code = code;
            return result;
        }

        public uint GetThreshold(uint Total)
        {
            return this.Code / (this.Range /= Total);
        }

        public void Init(Stream Stream)
        {
            // Stream.Init(stream);
            this.Stream = Stream;
            this.Code = 0;
            this.Range = 0xFFFFFFFF;
            for (int i = 0; i < 5; i++)
            {
                this.Code = (this.Code << 8) | (byte)this.Stream.ReadByte();
            }
        }

        public void Normalize()
        {
            while (this.Range < Decoder.kTopValue)
            {
                this.Code = (this.Code << 8) | (byte)this.Stream.ReadByte();
                this.Range <<= 8;
            }
        }

        public void Normalize2()
        {
            if (this.Range < Decoder.kTopValue)
            {
                this.Code = (this.Code << 8) | (byte)this.Stream.ReadByte();
                this.Range <<= 8;
            }
        }

        public void ReleaseStream()
        {
            // Stream.ReleaseStream();
            this.Stream = null;
        }

        // ulong GetProcessedSize() {return Stream.GetProcessedSize(); }
    }
}