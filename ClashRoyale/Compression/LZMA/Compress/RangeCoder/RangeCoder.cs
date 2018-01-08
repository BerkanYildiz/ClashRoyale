namespace ClashRoyale.Compression.Lzma.Compress.RangeCoder
{
    using System.IO;

    internal class Encoder
    {
        public const uint kTopValue = 1 << 24;

        public ulong Low;

        public uint Range;

        private byte _cache;

        private uint _cacheSize;

        private long StartPosition;

        private Stream Stream;

        public void CloseStream()
        {
            this.Stream.Close();
        }

        public void Encode(uint start, uint size, uint total)
        {
            this.Low += start * (this.Range /= total);
            this.Range *= size;
            while (this.Range < Encoder.kTopValue)
            {
                this.Range <<= 8;
                this.ShiftLow();
            }
        }

        public void EncodeBit(uint size0, int numTotalBits, uint symbol)
        {
            uint newBound = (this.Range >> numTotalBits) * size0;
            if (symbol == 0)
            {
                this.Range = newBound;
            }
            else
            {
                this.Low += newBound;
                this.Range -= newBound;
            }

            while (this.Range < Encoder.kTopValue)
            {
                this.Range <<= 8;
                this.ShiftLow();
            }
        }

        public void EncodeDirectBits(uint v, int numTotalBits)
        {
            for (int i = numTotalBits - 1; i >= 0; i--)
            {
                this.Range >>= 1;
                if (((v >> i) & 1) == 1)
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
            return this._cacheSize + this.Stream.Position - this.StartPosition + 4;

            // (long)Stream.GetProcessedSize();
        }

        public void Init()
        {
            this.StartPosition = this.Stream.Position;
            this.Low = 0;
            this.Range = 0xFFFFFFFF;
            this._cacheSize = 1;
            this._cache = 0;
        }

        public void ReleaseStream()
        {
            this.Stream = null;
        }

        public void SetStream(Stream stream)
        {
            this.Stream = stream;
        }

        public void ShiftLow()
        {
            if ((uint)this.Low < 0xFF000000 || (uint)(this.Low >> 32) == 1)
            {
                byte temp = this._cache;
                do
                {
                    this.Stream.WriteByte((byte)(temp + (this.Low >> 32)));
                    temp = 0xFF;
                }
                while (--this._cacheSize != 0);

                this._cache = (byte)((uint)this.Low >> 24);
            }

            this._cacheSize++;
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

        public void Decode(uint start, uint size, uint total)
        {
            this.Code -= start * this.Range;
            this.Range *= size;
            this.Normalize();
        }

        public uint DecodeBit(uint size0, int numTotalBits)
        {
            uint newBound = (this.Range >> numTotalBits) * size0;
            uint symbol;
            if (this.Code < newBound)
            {
                symbol = 0;
                this.Range = newBound;
            }
            else
            {
                symbol = 1;
                this.Code -= newBound;
                this.Range -= newBound;
            }

            this.Normalize();
            return symbol;
        }

        public uint DecodeDirectBits(int numTotalBits)
        {
            uint range = this.Range;
            uint code = this.Code;
            uint result = 0;
            for (int i = numTotalBits; i > 0; i--)
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

        public uint GetThreshold(uint total)
        {
            return this.Code / (this.Range /= total);
        }

        public void Init(Stream stream)
        {
            // Stream.Init(stream);
            this.Stream = stream;
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