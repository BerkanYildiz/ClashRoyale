namespace ClashRoyale.Compression.LZMA.Compress.LZMA
{
    using System;
    using System.IO;

    using ClashRoyale.Compression.LZMA.Compress.LZ;
    using ClashRoyale.Compression.LZMA.Compress.RangeCoder;

    public class Decoder : ICoder, ISetDecoderProperties
    {
        private bool Solid;

        private uint MDictionarySize;

        private uint MDictionarySizeCheck;

        private readonly BitDecoder[] MIsMatchDecoders = new BitDecoder[Base.kNumStates << Base.kNumPosStatesBitsMax];

        private readonly BitDecoder[] MIsRep0LongDecoders = new BitDecoder[Base.kNumStates << Base.kNumPosStatesBitsMax];

        private readonly BitDecoder[] MIsRepDecoders = new BitDecoder[Base.kNumStates];

        private readonly BitDecoder[] MIsRepG0Decoders = new BitDecoder[Base.kNumStates];

        private readonly BitDecoder[] MIsRepG1Decoders = new BitDecoder[Base.kNumStates];

        private readonly BitDecoder[] MIsRepG2Decoders = new BitDecoder[Base.kNumStates];

        private readonly LenDecoder MLenDecoder = new LenDecoder();

        private readonly LiteralDecoder MLiteralDecoder = new LiteralDecoder();

        private readonly OutWindow MOutWindow = new OutWindow();

        private BitTreeDecoder MPosAlignDecoder = new BitTreeDecoder(Base.kNumAlignBits);

        private readonly BitDecoder[] MPosDecoders = new BitDecoder[Base.kNumFullDistances - Base.kEndPosModelIndex];

        private readonly BitTreeDecoder[] MPosSlotDecoder = new BitTreeDecoder[Base.kNumLenToPosStates];

        private uint MPosStateMask;

        private readonly RangeCoder.Decoder MRangeDecoder = new RangeCoder.Decoder();

        private readonly LenDecoder MRepLenDecoder = new LenDecoder();

        public Decoder()
        {
            this.MDictionarySize = 0xFFFFFFFF;
            for (int i = 0; i < Base.kNumLenToPosStates; i++)
            {
                this.MPosSlotDecoder[i] = new BitTreeDecoder(Base.kNumPosSlotBits);
            }
        }

        public void Code(Stream InStream, Stream OutStream, long InSize, long OutSize, ICodeProgress Progress)
        {
            this.Init(InStream, OutStream);
            Base.State state = new Base.State();
            state.Init();
            uint rep0 = 0, rep1 = 0, rep2 = 0, rep3 = 0;
            ulong NowPos64 = 0;
            ulong OutSize64 = (UInt64)OutSize;
            if (NowPos64 < OutSize64)
            {
                if (this.MIsMatchDecoders[state.Index << Base.kNumPosStatesBitsMax].Decode(this.MRangeDecoder) != 0)
                {
                    throw new DataErrorException();
                }

                state.UpdateChar();
                byte b = this.MLiteralDecoder.DecodeNormal(this.MRangeDecoder, 0, 0);
                this.MOutWindow.PutByte(b);
                NowPos64++;
            }

            while (NowPos64 < OutSize64)
            {
                {
                    // UInt64 next = Math.Min(nowPos64 + (1 << 18), outSize64); while(nowPos64 < next)
                    uint PosState = (uint)NowPos64 & this.MPosStateMask;
                    if (this.MIsMatchDecoders[(state.Index << Base.kNumPosStatesBitsMax) + PosState].Decode(this.MRangeDecoder) == 0)
                    {
                        byte b;
                        byte PrevByte = this.MOutWindow.GetByte(0);
                        if (!state.IsCharState())
                        {
                            b = this.MLiteralDecoder.DecodeWithMatchByte(this.MRangeDecoder, (uint)NowPos64, PrevByte, this.MOutWindow.GetByte(rep0));
                        }
                        else
                        {
                            b = this.MLiteralDecoder.DecodeNormal(this.MRangeDecoder, (uint)NowPos64, PrevByte);
                        }

                        this.MOutWindow.PutByte(b);
                        state.UpdateChar();
                        NowPos64++;
                    }
                    else
                    {
                        uint len;
                        if (this.MIsRepDecoders[state.Index].Decode(this.MRangeDecoder) == 1)
                        {
                            if (this.MIsRepG0Decoders[state.Index].Decode(this.MRangeDecoder) == 0)
                            {
                                if (this.MIsRep0LongDecoders[(state.Index << Base.kNumPosStatesBitsMax) + PosState].Decode(this.MRangeDecoder) == 0)
                                {
                                    state.UpdateShortRep();
                                    this.MOutWindow.PutByte(this.MOutWindow.GetByte(rep0));
                                    NowPos64++;
                                    continue;
                                }
                            }
                            else
                            {
                                uint distance;
                                if (this.MIsRepG1Decoders[state.Index].Decode(this.MRangeDecoder) == 0)
                                {
                                    distance = rep1;
                                }
                                else
                                {
                                    if (this.MIsRepG2Decoders[state.Index].Decode(this.MRangeDecoder) == 0)
                                    {
                                        distance = rep2;
                                    }
                                    else
                                    {
                                        distance = rep3;
                                        rep3 = rep2;
                                    }

                                    rep2 = rep1;
                                }

                                rep1 = rep0;
                                rep0 = distance;
                            }

                            len = this.MRepLenDecoder.Decode(this.MRangeDecoder, PosState) + Base.kMatchMinLen;
                            state.UpdateRep();
                        }
                        else
                        {
                            rep3 = rep2;
                            rep2 = rep1;
                            rep1 = rep0;
                            len = Base.kMatchMinLen + this.MLenDecoder.Decode(this.MRangeDecoder, PosState);
                            state.UpdateMatch();
                            uint PosSlot = this.MPosSlotDecoder[Base.GetLenToPosState(len)].Decode(this.MRangeDecoder);
                            if (PosSlot >= Base.kStartPosModelIndex)
                            {
                                int NumDirectBits = (int)((PosSlot >> 1) - 1);
                                rep0 = (2 | (PosSlot & 1)) << NumDirectBits;
                                if (PosSlot < Base.kEndPosModelIndex)
                                {
                                    rep0 += BitTreeDecoder.ReverseDecode(this.MPosDecoders, rep0 - PosSlot - 1, this.MRangeDecoder, NumDirectBits);
                                }
                                else
                                {
                                    rep0 += this.MRangeDecoder.DecodeDirectBits(NumDirectBits - Base.kNumAlignBits) << Base.kNumAlignBits;
                                    rep0 += this.MPosAlignDecoder.ReverseDecode(this.MRangeDecoder);
                                }
                            }
                            else
                            {
                                rep0 = PosSlot;
                            }
                        }

                        if (rep0 >= this.MOutWindow.TrainSize + NowPos64 || rep0 >= this.MDictionarySizeCheck)
                        {
                            if (rep0 == 0xFFFFFFFF)
                            {
                                break;
                            }

                            throw new DataErrorException();
                        }

                        this.MOutWindow.CopyBlock(rep0, len);
                        NowPos64 += len;
                    }
                }
            }

            this.MOutWindow.Flush();
            this.MOutWindow.ReleaseStream();
            this.MRangeDecoder.ReleaseStream();
        }

        public void SetDecoderProperties(byte[] Properties)
        {
            if (Properties.Length < 5)
            {
                throw new InvalidParamException();
            }

            int lc = Properties[0] % 9;
            int remainder = Properties[0] / 9;
            int lp = remainder % 5;
            int pb = remainder / 5;
            if (pb > Base.kNumPosStatesBitsMax)
            {
                throw new InvalidParamException();
            }

            uint DictionarySize = 0;
            for (int i = 0; i < 4; i++)
            {
                DictionarySize += (UInt32)Properties[1 + i] << (i * 8);
            }

            this.SetDictionarySize(DictionarySize);
            this.SetLiteralProperties(lp, lc);
            this.SetPosBitsProperties(pb);
        }

        public bool Train(Stream Stream)
        {
            this.Solid = true;
            return this.MOutWindow.Train(Stream);
        }

        private void Init(Stream InStream, Stream OutStream)
        {
            this.MRangeDecoder.Init(InStream);
            this.MOutWindow.Init(OutStream, this.Solid);
            uint i;
            for (i = 0; i < Base.kNumStates; i++)
            {
                for (uint j = 0; j <= this.MPosStateMask; j++)
                {
                    uint index = (i << Base.kNumPosStatesBitsMax) + j;
                    this.MIsMatchDecoders[index].Init();
                    this.MIsRep0LongDecoders[index].Init();
                }

                this.MIsRepDecoders[i].Init();
                this.MIsRepG0Decoders[i].Init();
                this.MIsRepG1Decoders[i].Init();
                this.MIsRepG2Decoders[i].Init();
            }

            this.MLiteralDecoder.Init();
            for (i = 0; i < Base.kNumLenToPosStates; i++)
            {
                this.MPosSlotDecoder[i].Init();
            }

            // m_PosSpecDecoder.Init();
            for (i = 0; i < Base.kNumFullDistances - Base.kEndPosModelIndex; i++)
            {
                this.MPosDecoders[i].Init();
            }

            this.MLenDecoder.Init();
            this.MRepLenDecoder.Init();
            this.MPosAlignDecoder.Init();
        }

        private void SetDictionarySize(uint DictionarySize)
        {
            if (this.MDictionarySize != DictionarySize)
            {
                this.MDictionarySize = DictionarySize;
                this.MDictionarySizeCheck = Math.Max(this.MDictionarySize, 1);
                uint BlockSize = Math.Max(this.MDictionarySizeCheck, 1 << 12);
                this.MOutWindow.Create(BlockSize);
            }
        }

        private void SetLiteralProperties(int Lp, int Lc)
        {
            if (Lp > 8)
            {
                throw new InvalidParamException();
            }

            if (Lc > 8)
            {
                throw new InvalidParamException();
            }

            this.MLiteralDecoder.Create(Lp, Lc);
        }

        private void SetPosBitsProperties(int Pb)
        {
            if (Pb > Base.kNumPosStatesBitsMax)
            {
                throw new InvalidParamException();
            }

            uint NumPosStates = (uint)1 << Pb;
            this.MLenDecoder.Create(NumPosStates);
            this.MRepLenDecoder.Create(NumPosStates);
            this.MPosStateMask = NumPosStates - 1;
        }

        // ,System.IO.Stream
        private class LenDecoder
        {
            private BitDecoder MChoice = new BitDecoder();

            private BitDecoder MChoice2 = new BitDecoder();

            private BitTreeDecoder MHighCoder = new BitTreeDecoder(Base.kNumHighLenBits);

            private readonly BitTreeDecoder[] MLowCoder = new BitTreeDecoder[Base.kNumPosStatesMax];

            private readonly BitTreeDecoder[] MMidCoder = new BitTreeDecoder[Base.kNumPosStatesMax];

            private uint MNumPosStates;

            public void Create(uint NumPosStates)
            {
                for (uint PosState = this.MNumPosStates; PosState < NumPosStates; PosState++)
                {
                    this.MLowCoder[PosState] = new BitTreeDecoder(Base.kNumLowLenBits);
                    this.MMidCoder[PosState] = new BitTreeDecoder(Base.kNumMidLenBits);
                }

                this.MNumPosStates = NumPosStates;
            }

            public uint Decode(RangeCoder.Decoder RangeDecoder, uint PosState)
            {
                if (this.MChoice.Decode(RangeDecoder) == 0)
                {
                    return this.MLowCoder[PosState].Decode(RangeDecoder);
                }

                uint symbol = Base.kNumLowLenSymbols;
                if (this.MChoice2.Decode(RangeDecoder) == 0)
                {
                    symbol += this.MMidCoder[PosState].Decode(RangeDecoder);
                }
                else
                {
                    symbol += Base.kNumMidLenSymbols;
                    symbol += this.MHighCoder.Decode(RangeDecoder);
                }

                return symbol;
            }

            public void Init()
            {
                this.MChoice.Init();
                for (uint PosState = 0; PosState < this.MNumPosStates; PosState++)
                {
                    this.MLowCoder[PosState].Init();
                    this.MMidCoder[PosState].Init();
                }

                this.MChoice2.Init();
                this.MHighCoder.Init();
            }
        }

        private class LiteralDecoder
        {
            private Decoder2[] MCoders;

            private int MNumPosBits;

            private int MNumPrevBits;

            private uint MPosMask;

            public void Create(int NumPosBits, int NumPrevBits)
            {
                if (this.MCoders != null && this.MNumPrevBits == NumPrevBits && this.MNumPosBits == NumPosBits)
                {
                    return;
                }

                this.MNumPosBits = NumPosBits;
                this.MPosMask = ((uint)1 << NumPosBits) - 1;
                this.MNumPrevBits = NumPrevBits;
                uint NumStates = (uint)1 << (this.MNumPrevBits + this.MNumPosBits);
                this.MCoders = new Decoder2[NumStates];
                for (uint i = 0; i < NumStates; i++)
                {
                    this.MCoders[i].Create();
                }
            }

            public byte DecodeNormal(RangeCoder.Decoder RangeDecoder, uint Pos, byte PrevByte)
            {
                return this.MCoders[this.GetState(Pos, PrevByte)].DecodeNormal(RangeDecoder);
            }

            public byte DecodeWithMatchByte(RangeCoder.Decoder RangeDecoder, uint Pos, byte PrevByte, byte MatchByte)
            {
                return this.MCoders[this.GetState(Pos, PrevByte)].DecodeWithMatchByte(RangeDecoder, MatchByte);
            }

            public void Init()
            {
                uint NumStates = (uint)1 << (this.MNumPrevBits + this.MNumPosBits);
                for (uint i = 0; i < NumStates; i++)
                {
                    this.MCoders[i].Init();
                }
            }

            private uint GetState(uint Pos, byte PrevByte)
            {
                return ((Pos & this.MPosMask) << this.MNumPrevBits) + (uint)(PrevByte >> (8 - this.MNumPrevBits));
            }

            private struct Decoder2
            {
                private BitDecoder[] MDecoders;

                public void Create()
                {
                    this.MDecoders = new BitDecoder[0x300];
                }

                public void Init()
                {
                    for (int i = 0; i < 0x300; i++)
                    {
                        this.MDecoders[i].Init();
                    }
                }

                public byte DecodeNormal(RangeCoder.Decoder RangeDecoder)
                {
                    uint symbol = 1;
                    do
                    {
                        symbol = (symbol << 1) | this.MDecoders[symbol].Decode(RangeDecoder);
                    }
                    while (symbol < 0x100);

                    return (byte)symbol;
                }

                public byte DecodeWithMatchByte(RangeCoder.Decoder RangeDecoder, byte MatchByte)
                {
                    uint symbol = 1;
                    do
                    {
                        uint MatchBit = (uint)(MatchByte >> 7) & 1;
                        MatchByte <<= 1;
                        uint bit = this.MDecoders[((1 + MatchBit) << 8) + symbol].Decode(RangeDecoder);
                        symbol = (symbol << 1) | bit;
                        if (MatchBit != bit)
                        {
                            while (symbol < 0x100)
                            {
                                symbol = (symbol << 1) | this.MDecoders[symbol].Decode(RangeDecoder);
                            }

                            break;
                        }
                    }
                    while (symbol < 0x100);

                    return (byte)symbol;
                }
            }
        }

        /*
		public override bool CanRead { get { return true; }}
		public override bool CanWrite { get { return true; }}
		public override bool CanSeek { get { return true; }}
		public override long Length { get { return 0; }}
		public override long Position
		{
			get { return 0;	}
			set { }
		}
		public override void Flush() { }
		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
		}
		public override long Seek(long offset, System.IO.SeekOrigin origin)
		{
			return 0;
		}
		public override void SetLength(long value) {}
		*/
    }
}