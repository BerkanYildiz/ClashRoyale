namespace ClashRoyale.Compression.LZMA.Compress.RangeCoder
{
    internal struct BitTreeEncoder
    {
        private readonly BitEncoder[] Models;

        private readonly int NumBitLevels;

        public BitTreeEncoder(int NumBitLevels)
        {
            this.NumBitLevels = NumBitLevels;
            this.Models = new BitEncoder[1 << NumBitLevels];
        }

        public void Init()
        {
            for (uint i = 1; i < 1 << this.NumBitLevels; i++)
            {
                this.Models[i].Init();
            }
        }

        public void Encode(Encoder RangeEncoder, uint Symbol)
        {
            uint m = 1;
            for (int BitIndex = this.NumBitLevels; BitIndex > 0;)
            {
                BitIndex--;
                uint bit = (Symbol >> BitIndex) & 1;
                this.Models[m].Encode(RangeEncoder, bit);
                m = (m << 1) | bit;
            }
        }

        public void ReverseEncode(Encoder RangeEncoder, uint Symbol)
        {
            uint m = 1;
            for (uint i = 0; i < this.NumBitLevels; i++)
            {
                uint bit = Symbol & 1;
                this.Models[m].Encode(RangeEncoder, bit);
                m = (m << 1) | bit;
                Symbol >>= 1;
            }
        }

        public uint GetPrice(uint Symbol)
        {
            uint price = 0;
            uint m = 1;
            for (int BitIndex = this.NumBitLevels; BitIndex > 0;)
            {
                BitIndex--;
                uint bit = (Symbol >> BitIndex) & 1;
                price += this.Models[m].GetPrice(bit);
                m = (m << 1) + bit;
            }

            return price;
        }

        public uint ReverseGetPrice(uint Symbol)
        {
            uint price = 0;
            uint m = 1;
            for (int i = this.NumBitLevels; i > 0; i--)
            {
                uint bit = Symbol & 1;
                Symbol >>= 1;
                price += this.Models[m].GetPrice(bit);
                m = (m << 1) | bit;
            }

            return price;
        }

        public static uint ReverseGetPrice(BitEncoder[] Models, uint StartIndex, int NumBitLevels, uint Symbol)
        {
            uint price = 0;
            uint m = 1;
            for (int i = NumBitLevels; i > 0; i--)
            {
                uint bit = Symbol & 1;
                Symbol >>= 1;
                price += Models[StartIndex + m].GetPrice(bit);
                m = (m << 1) | bit;
            }

            return price;
        }

        public static void ReverseEncode(BitEncoder[] Models, uint StartIndex, Encoder RangeEncoder, int NumBitLevels, uint Symbol)
        {
            uint m = 1;
            for (int i = 0; i < NumBitLevels; i++)
            {
                uint bit = Symbol & 1;
                Models[StartIndex + m].Encode(RangeEncoder, bit);
                m = (m << 1) | bit;
                Symbol >>= 1;
            }
        }
    }

    internal struct BitTreeDecoder
    {
        private readonly BitDecoder[] Models;

        private readonly int NumBitLevels;

        public BitTreeDecoder(int NumBitLevels)
        {
            this.NumBitLevels = NumBitLevels;
            this.Models = new BitDecoder[1 << NumBitLevels];
        }

        public void Init()
        {
            for (uint i = 1; i < 1 << this.NumBitLevels; i++)
            {
                this.Models[i].Init();
            }
        }

        public uint Decode(Decoder RangeDecoder)
        {
            uint m = 1;
            for (int BitIndex = this.NumBitLevels; BitIndex > 0; BitIndex--)
            {
                m = (m << 1) + this.Models[m].Decode(RangeDecoder);
            }

            return m - ((uint)1 << this.NumBitLevels);
        }

        public uint ReverseDecode(Decoder RangeDecoder)
        {
            uint m = 1;
            uint symbol = 0;
            for (int BitIndex = 0; BitIndex < this.NumBitLevels; BitIndex++)
            {
                uint bit = this.Models[m].Decode(RangeDecoder);
                m <<= 1;
                m += bit;
                symbol |= bit << BitIndex;
            }

            return symbol;
        }

        public static uint ReverseDecode(BitDecoder[] Models, uint StartIndex, Decoder RangeDecoder, int NumBitLevels)
        {
            uint m = 1;
            uint symbol = 0;
            for (int BitIndex = 0; BitIndex < NumBitLevels; BitIndex++)
            {
                uint bit = Models[StartIndex + m].Decode(RangeDecoder);
                m <<= 1;
                m += bit;
                symbol |= bit << BitIndex;
            }

            return symbol;
        }
    }
}