namespace ClashRoyale.Compression.LZMA.Compress.RangeCoder
{
    using System;

    internal struct BitEncoder
    {
        public const int kNumBitModelTotalBits = 11;

        public const uint kBitModelTotal = 1 << BitEncoder.kNumBitModelTotalBits;

        private const int kNumMoveBits = 5;

        private const int kNumMoveReducingBits = 2;

        public const int kNumBitPriceShiftBits = 6;

        private uint Prob;

        public void Init()
        {
            this.Prob = BitEncoder.kBitModelTotal >> 1;
        }

        public void UpdateModel(uint Symbol)
        {
            if (Symbol == 0)
            {
                this.Prob += (BitEncoder.kBitModelTotal - this.Prob) >> BitEncoder.kNumMoveBits;
            }
            else
            {
                this.Prob -= this.Prob >> BitEncoder.kNumMoveBits;
            }
        }

        public void Encode(Encoder Encoder, uint Symbol)
        {
            // encoder.EncodeBit(Prob, kNumBitModelTotalBits, symbol); UpdateModel(symbol);
            uint NewBound = (Encoder.Range >> BitEncoder.kNumBitModelTotalBits) * this.Prob;
            if (Symbol == 0)
            {
                Encoder.Range = NewBound;
                this.Prob += (BitEncoder.kBitModelTotal - this.Prob) >> BitEncoder.kNumMoveBits;
            }
            else
            {
                Encoder.Low += NewBound;
                Encoder.Range -= NewBound;
                this.Prob -= this.Prob >> BitEncoder.kNumMoveBits;
            }

            if (Encoder.Range < Encoder.kTopValue)
            {
                Encoder.Range <<= 8;
                Encoder.ShiftLow();
            }
        }

        private static readonly uint[] ProbPrices = new uint[BitEncoder.kBitModelTotal >> BitEncoder.kNumMoveReducingBits];

        static BitEncoder()
        {
            const int KNumBits = BitEncoder.kNumBitModelTotalBits - BitEncoder.kNumMoveReducingBits;
            for (int i = KNumBits - 1; i >= 0; i--)
            {
                uint start = (UInt32)1 << (KNumBits - i - 1);
                uint end = (UInt32)1 << (KNumBits - i);
                for (uint j = start; j < end; j++)
                {
                    BitEncoder.ProbPrices[j] = ((UInt32)i << BitEncoder.kNumBitPriceShiftBits) + (((end - j) << BitEncoder.kNumBitPriceShiftBits) >> (KNumBits - i - 1));
                }
            }
        }

        public uint GetPrice(uint Symbol)
        {
            return BitEncoder.ProbPrices[(((this.Prob - Symbol) ^ -(int)Symbol) & (BitEncoder.kBitModelTotal - 1)) >> BitEncoder.kNumMoveReducingBits];
        }

        public uint GetPrice0()
        {
            return BitEncoder.ProbPrices[this.Prob >> BitEncoder.kNumMoveReducingBits];
        }

        public uint GetPrice1()
        {
            return BitEncoder.ProbPrices[(BitEncoder.kBitModelTotal - this.Prob) >> BitEncoder.kNumMoveReducingBits];
        }
    }

    internal struct BitDecoder
    {
        public const int kNumBitModelTotalBits = 11;

        public const uint kBitModelTotal = 1 << BitDecoder.kNumBitModelTotalBits;

        private const int kNumMoveBits = 5;

        private uint Prob;

        public void UpdateModel(int NumMoveBits, uint Symbol)
        {
            if (Symbol == 0)
            {
                this.Prob += (BitDecoder.kBitModelTotal - this.Prob) >> NumMoveBits;
            }
            else
            {
                this.Prob -= this.Prob >> NumMoveBits;
            }
        }

        public void Init()
        {
            this.Prob = BitDecoder.kBitModelTotal >> 1;
        }

        public uint Decode(Decoder RangeDecoder)
        {
            uint NewBound = (RangeDecoder.Range >> BitDecoder.kNumBitModelTotalBits) * this.Prob;
            if (RangeDecoder.Code < NewBound)
            {
                RangeDecoder.Range = NewBound;
                this.Prob += (BitDecoder.kBitModelTotal - this.Prob) >> BitDecoder.kNumMoveBits;
                if (RangeDecoder.Range < Decoder.kTopValue)
                {
                    RangeDecoder.Code = (RangeDecoder.Code << 8) | (byte)RangeDecoder.Stream.ReadByte();
                    RangeDecoder.Range <<= 8;
                }

                return 0;
            }

            RangeDecoder.Range -= NewBound;
            RangeDecoder.Code -= NewBound;
            this.Prob -= this.Prob >> BitDecoder.kNumMoveBits;
            if (RangeDecoder.Range < Decoder.kTopValue)
            {
                RangeDecoder.Code = (RangeDecoder.Code << 8) | (byte)RangeDecoder.Stream.ReadByte();
                RangeDecoder.Range <<= 8;
            }

            return 1;
        }
    }
}