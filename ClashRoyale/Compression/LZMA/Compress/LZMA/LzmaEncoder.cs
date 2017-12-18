namespace ClashRoyale.Compression.LZMA.Compress.LZMA
{
    using System;
    using System.IO;

    using ClashRoyale.Compression.LZMA.Compress.LZ;
    using ClashRoyale.Compression.LZMA.Compress.RangeCoder;

    public class Encoder : ICoder, ISetCoderProperties, IWriteCoderProperties
    {
        private const int kDefaultDictionaryLogSize = 22;

        private const uint kIfinityPrice = 0xFFFFFFF;

        private const uint kNumFastBytesDefault = 0x20;

        private const uint kNumLenSpecSymbols = Base.kNumLowLenSymbols + Base.kNumMidLenSymbols;

        private const uint kNumOpts = 1 << 12;

        private const int kPropSize = 5;

        private static readonly byte[] g_FastPos = new byte[1 << 11];

        private static readonly string[] kMatchFinderIDs =
            {
                "BT2", "BT4"
            };

        private uint AdditionalOffset;

        private uint AlignPriceCount;

        private readonly uint[] AlignPrices = new uint[Base.kAlignTableSize];

        private uint DictionarySize = 1 << Encoder.kDefaultDictionaryLogSize;

        private uint DictionarySizePrev = 0xFFFFFFFF;

        private readonly uint[] DistancesPrices = new uint[Base.kNumFullDistances << Base.kNumLenToPosStatesBits];

        private uint DistTableSize = Encoder.kDefaultDictionaryLogSize * 2;

        private bool Finished;

        private Stream InStream;

        private readonly BitEncoder[] IsMatch = new BitEncoder[Base.kNumStates << Base.kNumPosStatesBitsMax];

        private readonly BitEncoder[] IsRep = new BitEncoder[Base.kNumStates];

        private readonly BitEncoder[] IsRep0Long = new BitEncoder[Base.kNumStates << Base.kNumPosStatesBitsMax];

        private readonly BitEncoder[] IsRepG0 = new BitEncoder[Base.kNumStates];

        private readonly BitEncoder[] IsRepG1 = new BitEncoder[Base.kNumStates];

        private readonly BitEncoder[] IsRepG2 = new BitEncoder[Base.kNumStates];

        private readonly LenPriceTableEncoder _lenEncoder = new LenPriceTableEncoder();

        private readonly LiteralEncoder _literalEncoder = new LiteralEncoder();

        private uint LongestMatchLength;

        private bool LongestMatchWasFound;

        private readonly uint[] MatchDistances = new uint[Base.kMatchMaxLen * 2 + 2];

        private IMatchFinder MatchFinder;

        private EMatchFinderType MatchFinderType = EMatchFinderType.Bt4;

        private uint MatchPriceCount;

        private bool NeedReleaseMfStream;

        private uint NumDistancePairs;

        private uint NumFastBytes = Encoder.kNumFastBytesDefault;

        private uint NumFastBytesPrev = 0xFFFFFFFF;

        private int NumLiteralContextBits = 3;

        private int NumLiteralPosStateBits;

        private readonly Optimal[] Optimum = new Optimal[Encoder.kNumOpts];

        private uint OptimumCurrentIndex;

        private uint OptimumEndIndex;

        private BitTreeEncoder PosAlignEncoder = new BitTreeEncoder(Base.kNumAlignBits);

        private readonly BitEncoder[] PosEncoders = new BitEncoder[Base.kNumFullDistances - Base.kEndPosModelIndex];

        private readonly BitTreeEncoder[] PosSlotEncoder = new BitTreeEncoder[Base.kNumLenToPosStates];

        private readonly uint[] PosSlotPrices = new uint[1 << (Base.kNumPosSlotBits + Base.kNumLenToPosStatesBits)];

        private int PosStateBits = 2;

        private uint PosStateMask = 4 - 1;

        private byte Previoubyte;

        private readonly RangeCoder.Encoder RangeEncoder = new RangeCoder.Encoder();

        private readonly uint[] RepDistances = new uint[Base.kNumRepDistances];

        private readonly LenPriceTableEncoder RepMatchLenEncoder = new LenPriceTableEncoder();

        private Base.State State = new Base.State();

        private uint TrainSize;

        private bool WriteEndMark;

        private long NowPos64;

        private readonly byte[] Properties = new byte[Encoder.kPropSize];

        private readonly uint[] RepLens = new uint[Base.kNumRepDistances];

        private readonly uint[] Reps = new uint[Base.kNumRepDistances];

        private readonly uint[] TempPrices = new uint[Base.kNumFullDistances];

        private enum EMatchFinderType
        {
            Bt2,

            Bt4
        }

        static Encoder()
        {
            const byte KFastSlots = 22;
            int c = 2;
            Encoder.g_FastPos[0] = 0;
            Encoder.g_FastPos[1] = 1;
            for (byte SlotFast = 2; SlotFast < KFastSlots; SlotFast++)
            {
                uint k = (UInt32)1 << ((SlotFast >> 1) - 1);
                for (uint j = 0; j < k; j++, c++)
                {
                    Encoder.g_FastPos[c] = SlotFast;
                }
            }
        }

        public Encoder()
        {
            for (int i = 0; i < Encoder.kNumOpts; i++)
            {
                this.Optimum[i] = new Optimal();
            }

            for (int i = 0; i < Base.kNumLenToPosStates; i++)
            {
                this.PosSlotEncoder[i] = new BitTreeEncoder(Base.kNumPosSlotBits);
            }
        }

        public void Code(Stream InStream, Stream OutStream, long InSize, long OutSize, ICodeProgress Progress)
        {
            this.NeedReleaseMfStream = false;
            try
            {
                this.SetStreams(InStream, OutStream, InSize, OutSize);
                while (true)
                {
                    long ProcessedInSize;
                    long ProcessedOutSize;
                    bool finished;
                    this.CodeOneBlock(out ProcessedInSize, out ProcessedOutSize, out finished);
                    if (finished)
                    {
                        return;
                    }

                    if (Progress != null)
                    {
                        Progress.SetProgress(ProcessedInSize, ProcessedOutSize);
                    }
                }
            }
            finally
            {
                this.ReleaseStreams();
            }
        }

        public void CodeOneBlock(out long InSize, out long OutSize, out bool Finished)
        {
            InSize = 0;
            OutSize = 0;
            Finished = true;
            if (this.InStream != null)
            {
                this.MatchFinder.SetStream(this.InStream);
                this.MatchFinder.Init();
                this.NeedReleaseMfStream = true;
                this.InStream = null;
                if (this.TrainSize > 0)
                {
                    this.MatchFinder.Skip(this.TrainSize);
                }
            }

            if (this.Finished)
            {
                return;
            }

            this.Finished = true;
            long ProgressPosValuePrev = this.NowPos64;
            if (this.NowPos64 == 0)
            {
                if (this.MatchFinder.GetNumAvailableBytes() == 0)
                {
                    this.Flush((UInt32)this.NowPos64);
                    return;
                }

                uint len, NumDistancePairs; // it's not used
                this.ReadMatchDistances(out len, out NumDistancePairs);
                uint PosState = (UInt32)this.NowPos64 & this.PosStateMask;
                this.IsMatch[(this.State.Index << Base.kNumPosStatesBitsMax) + PosState].Encode(this.RangeEncoder, 0);
                this.State.UpdateChar();
                byte CurByte = this.MatchFinder.GetIndexByte((Int32)(0 - this.AdditionalOffset));
                this._literalEncoder.GetSubCoder((UInt32)this.NowPos64, this.Previoubyte).Encode(this.RangeEncoder, CurByte);
                this.Previoubyte = CurByte;
                this.AdditionalOffset--;
                this.NowPos64++;
            }

            if (this.MatchFinder.GetNumAvailableBytes() == 0)
            {
                this.Flush((UInt32)this.NowPos64);
                return;
            }

            while (true)
            {
                uint pos;
                uint len = this.GetOptimum((UInt32)this.NowPos64, out pos);
                uint PosState = (UInt32)this.NowPos64 & this.PosStateMask;
                uint ComplexState = (this.State.Index << Base.kNumPosStatesBitsMax) + PosState;
                if (len == 1 && pos == 0xFFFFFFFF)
                {
                    this.IsMatch[ComplexState].Encode(this.RangeEncoder, 0);
                    byte CurByte = this.MatchFinder.GetIndexByte((Int32)(0 - this.AdditionalOffset));
                    LiteralEncoder.Encoder2 SubCoder = this._literalEncoder.GetSubCoder((UInt32)this.NowPos64, this.Previoubyte);
                    if (!this.State.IsCharState())
                    {
                        byte MatchByte = this.MatchFinder.GetIndexByte((Int32)(0 - this.RepDistances[0] - 1 - this.AdditionalOffset));
                        SubCoder.EncodeMatched(this.RangeEncoder, MatchByte, CurByte);
                    }
                    else
                    {
                        SubCoder.Encode(this.RangeEncoder, CurByte);
                    }

                    this.Previoubyte = CurByte;
                    this.State.UpdateChar();
                }
                else
                {
                    this.IsMatch[ComplexState].Encode(this.RangeEncoder, 1);
                    if (pos < Base.kNumRepDistances)
                    {
                        this.IsRep[this.State.Index].Encode(this.RangeEncoder, 1);
                        if (pos == 0)
                        {
                            this.IsRepG0[this.State.Index].Encode(this.RangeEncoder, 0);
                            if (len == 1)
                            {
                                this.IsRep0Long[ComplexState].Encode(this.RangeEncoder, 0);
                            }
                            else
                            {
                                this.IsRep0Long[ComplexState].Encode(this.RangeEncoder, 1);
                            }
                        }
                        else
                        {
                            this.IsRepG0[this.State.Index].Encode(this.RangeEncoder, 1);
                            if (pos == 1)
                            {
                                this.IsRepG1[this.State.Index].Encode(this.RangeEncoder, 0);
                            }
                            else
                            {
                                this.IsRepG1[this.State.Index].Encode(this.RangeEncoder, 1);
                                this.IsRepG2[this.State.Index].Encode(this.RangeEncoder, pos - 2);
                            }
                        }

                        if (len == 1)
                        {
                            this.State.UpdateShortRep();
                        }
                        else
                        {
                            this.RepMatchLenEncoder.Encode(this.RangeEncoder, len - Base.kMatchMinLen, PosState);
                            this.State.UpdateRep();
                        }

                        uint distance = this.RepDistances[pos];
                        if (pos != 0)
                        {
                            for (uint i = pos; i >= 1; i--)
                            {
                                this.RepDistances[i] = this.RepDistances[i - 1];
                            }

                            this.RepDistances[0] = distance;
                        }
                    }
                    else
                    {
                        this.IsRep[this.State.Index].Encode(this.RangeEncoder, 0);
                        this.State.UpdateMatch();
                        this._lenEncoder.Encode(this.RangeEncoder, len - Base.kMatchMinLen, PosState);
                        pos -= Base.kNumRepDistances;
                        uint PosSlot = Encoder.GetPosSlot(pos);
                        uint LenToPosState = Base.GetLenToPosState(len);
                        this.PosSlotEncoder[LenToPosState].Encode(this.RangeEncoder, PosSlot);
                        if (PosSlot >= Base.kStartPosModelIndex)
                        {
                            int FooterBits = (int)((PosSlot >> 1) - 1);
                            uint BaseVal = (2 | (PosSlot & 1)) << FooterBits;
                            uint PosReduced = pos - BaseVal;
                            if (PosSlot < Base.kEndPosModelIndex)
                            {
                                BitTreeEncoder.ReverseEncode(this.PosEncoders, BaseVal - PosSlot - 1, this.RangeEncoder, FooterBits, PosReduced);
                            }
                            else
                            {
                                this.RangeEncoder.EncodeDirectBits(PosReduced >> Base.kNumAlignBits, FooterBits - Base.kNumAlignBits);
                                this.PosAlignEncoder.ReverseEncode(this.RangeEncoder, PosReduced & Base.kAlignMask);
                                this.AlignPriceCount++;
                            }
                        }

                        uint distance = pos;
                        for (uint i = Base.kNumRepDistances - 1; i >= 1; i--)
                        {
                            this.RepDistances[i] = this.RepDistances[i - 1];
                        }

                        this.RepDistances[0] = distance;
                        this.MatchPriceCount++;
                    }

                    this.Previoubyte = this.MatchFinder.GetIndexByte((Int32)(len - 1 - this.AdditionalOffset));
                }

                this.AdditionalOffset -= len;
                this.NowPos64 += len;
                if (this.AdditionalOffset == 0)
                {
                    // if (!_fastMode)
                    if (this.MatchPriceCount >= 1 << 7)
                    {
                        this.FillDistancesPrices();
                    }

                    if (this.AlignPriceCount >= Base.kAlignTableSize)
                    {
                        this.FillAlignPrices();
                    }

                    InSize = this.NowPos64;
                    OutSize = this.RangeEncoder.GetProcessedSizeAdd();
                    if (this.MatchFinder.GetNumAvailableBytes() == 0)
                    {
                        this.Flush((UInt32)this.NowPos64);
                        return;
                    }

                    if (this.NowPos64 - ProgressPosValuePrev >= 1 << 12)
                    {
                        this.Finished = false;
                        Finished = false;
                        return;
                    }
                }
            }
        }

        public void SetCoderProperties(CoderPropId[] PropIDs, object[] Properties)
        {
            for (uint i = 0; i < Properties.Length; i++)
            {
                object prop = Properties[i];
                switch (PropIDs[i])
                {
                    case CoderPropId.NumFastBytes:
                        {
                            if (!(prop is Int32))
                            {
                                throw new InvalidParamException();
                            }

                            int NumFastBytes = (Int32)prop;
                            if (NumFastBytes < 5 || NumFastBytes > Base.kMatchMaxLen)
                            {
                                throw new InvalidParamException();
                            }

                            this.NumFastBytes = (UInt32)NumFastBytes;
                            break;
                        }

                    case CoderPropId.Algorithm:
                        {
                            /*
                            if (!(prop is Int32))
                                throw new InvalidParamException();
                            Int32 maximize = (Int32)prop;
                            _fastMode = (maximize == 0);
                            _maxMode = (maximize >= 2);
                            */
                            break;
                        }

                    case CoderPropId.MatchFinder:
                        {
                            if (!(prop is String))
                            {
                                throw new InvalidParamException();
                            }

                            EMatchFinderType MatchFinderIndexPrev = this.MatchFinderType;
                            int m = Encoder.FindMatchFinder(((string)prop).ToUpper());
                            if (m < 0)
                            {
                                throw new InvalidParamException();
                            }

                            this.MatchFinderType = (EMatchFinderType)m;
                            if (this.MatchFinder != null && MatchFinderIndexPrev != this.MatchFinderType)
                            {
                                this.DictionarySizePrev = 0xFFFFFFFF;
                                this.MatchFinder = null;
                            }

                            break;
                        }

                    case CoderPropId.DictionarySize:
                        {
                            const int KDicLogSizeMaxCompress = 30;
                            if (!(prop is Int32))
                            {
                                throw new InvalidParamException();
                            }

                            int DictionarySize = (Int32)prop;
                            if (DictionarySize < (UInt32)(1 << Base.kDicLogSizeMin) || DictionarySize > (UInt32)(1 << KDicLogSizeMaxCompress))
                            {
                                throw new InvalidParamException();
                            }

                            this.DictionarySize = (UInt32)DictionarySize;
                            int DicLogSize;
                            for (DicLogSize = 0; DicLogSize < (UInt32)KDicLogSizeMaxCompress; DicLogSize++)
                            {
                                if (DictionarySize <= (UInt32)1 << DicLogSize)
                                {
                                    break;
                                }
                            }

                            this.DistTableSize = (UInt32)DicLogSize * 2;
                            break;
                        }

                    case CoderPropId.PosStateBits:
                        {
                            if (!(prop is Int32))
                            {
                                throw new InvalidParamException();
                            }

                            int v = (Int32)prop;
                            if (v < 0 || v > (UInt32)Base.kNumPosStatesBitsEncodingMax)
                            {
                                throw new InvalidParamException();
                            }

                            this.PosStateBits = v;
                            this.PosStateMask = ((UInt32)1 << this.PosStateBits) - 1;
                            break;
                        }

                    case CoderPropId.LitPosBits:
                        {
                            if (!(prop is Int32))
                            {
                                throw new InvalidParamException();
                            }

                            int v = (Int32)prop;
                            if (v < 0 || v > Base.kNumLitPosStatesBitsEncodingMax)
                            {
                                throw new InvalidParamException();
                            }

                            this.NumLiteralPosStateBits = v;
                            break;
                        }

                    case CoderPropId.LitContextBits:
                        {
                            if (!(prop is Int32))
                            {
                                throw new InvalidParamException();
                            }

                            int v = (Int32)prop;
                            if (v < 0 || v > Base.kNumLitContextBitsMax)
                            {
                                throw new InvalidParamException();
                            }

                            this.NumLiteralContextBits = v;
                            break;
                        }

                    case CoderPropId.EndMarker:
                        {
                            if (!(prop is Boolean))
                            {
                                throw new InvalidParamException();
                            }

                            this.SetWriteEndMarkerMode((Boolean)prop);
                            break;
                        }

                    default: throw new InvalidParamException();
                }
            }
        }

        public void SetTrainSize(uint TrainSize)
        {
            this.TrainSize = TrainSize;
        }

        public void WriteCoderProperties(Stream OutStream)
        {
            this.Properties[0] = (Byte)((this.PosStateBits * 5 + this.NumLiteralPosStateBits) * 9 + this.NumLiteralContextBits);
            for (int i = 0; i < 4; i++)
            {
                this.Properties[1 + i] = (Byte)((this.DictionarySize >> (8 * i)) & 0xFF);
            }

            OutStream.Write(this.Properties, 0, Encoder.kPropSize);
        }

        private static int FindMatchFinder(string S)
        {
            for (int m = 0; m < Encoder.kMatchFinderIDs.Length; m++)
            {
                if (S == Encoder.kMatchFinderIDs[m])
                {
                    return m;
                }
            }

            return -1;
        }

        private static uint GetPosSlot(uint Pos)
        {
            if (Pos < 1 << 11)
            {
                return Encoder.g_FastPos[Pos];
            }

            if (Pos < 1 << 21)
            {
                return (UInt32)(Encoder.g_FastPos[Pos >> 10] + 20);
            }

            return (UInt32)(Encoder.g_FastPos[Pos >> 20] + 40);
        }

        private static uint GetPosSlot2(uint Pos)
        {
            if (Pos < 1 << 17)
            {
                return (UInt32)(Encoder.g_FastPos[Pos >> 6] + 12);
            }

            if (Pos < 1 << 27)
            {
                return (UInt32)(Encoder.g_FastPos[Pos >> 16] + 32);
            }

            return (UInt32)(Encoder.g_FastPos[Pos >> 26] + 52);
        }

        private uint Backward(out uint BackRes, uint Cur)
        {
            this.OptimumEndIndex = Cur;
            uint PosMem = this.Optimum[Cur].PosPrev;
            uint BackMem = this.Optimum[Cur].BackPrev;
            do
            {
                if (this.Optimum[Cur].Prev1IsChar)
                {
                    this.Optimum[PosMem].MakeAsChar();
                    this.Optimum[PosMem].PosPrev = PosMem - 1;
                    if (this.Optimum[Cur].Prev2)
                    {
                        this.Optimum[PosMem - 1].Prev1IsChar = false;
                        this.Optimum[PosMem - 1].PosPrev = this.Optimum[Cur].PosPrev2;
                        this.Optimum[PosMem - 1].BackPrev = this.Optimum[Cur].BackPrev2;
                    }
                }

                uint PosPrev = PosMem;
                uint BackCur = BackMem;
                BackMem = this.Optimum[PosPrev].BackPrev;
                PosMem = this.Optimum[PosPrev].PosPrev;
                this.Optimum[PosPrev].BackPrev = BackCur;
                this.Optimum[PosPrev].PosPrev = Cur;
                Cur = PosPrev;
            }
            while (Cur > 0);

            BackRes = this.Optimum[0].BackPrev;
            this.OptimumCurrentIndex = this.Optimum[0].PosPrev;
            return this.OptimumCurrentIndex;
        }

        private void BaseInit()
        {
            this.State.Init();
            this.Previoubyte = 0;
            for (uint i = 0; i < Base.kNumRepDistances; i++)
            {
                this.RepDistances[i] = 0;
            }
        }

        private bool ChangePair(uint SmallDist, uint BigDist)
        {
            const int KDif = 7;
            return SmallDist < (UInt32)1 << (32 - KDif) && BigDist >= SmallDist << KDif;
        }

        private void Create()
        {
            if (this.MatchFinder == null)
            {
                BinTree bt = new BinTree();
                int NumHashBytes = 4;
                if (this.MatchFinderType == EMatchFinderType.Bt2)
                {
                    NumHashBytes = 2;
                }

                bt.SetType(NumHashBytes);
                this.MatchFinder = bt;
            }

            this._literalEncoder.Create(this.NumLiteralPosStateBits, this.NumLiteralContextBits);
            if (this.DictionarySize == this.DictionarySizePrev && this.NumFastBytesPrev == this.NumFastBytes)
            {
                return;
            }

            this.MatchFinder.Create(this.DictionarySize, Encoder.kNumOpts, this.NumFastBytes, Base.kMatchMaxLen + 1);
            this.DictionarySizePrev = this.DictionarySize;
            this.NumFastBytesPrev = this.NumFastBytes;
        }

        private void FillAlignPrices()
        {
            for (uint i = 0; i < Base.kAlignTableSize; i++)
            {
                this.AlignPrices[i] = this.PosAlignEncoder.ReverseGetPrice(i);
            }

            this.AlignPriceCount = 0;
        }

        private void FillDistancesPrices()
        {
            for (uint i = Base.kStartPosModelIndex; i < Base.kNumFullDistances; i++)
            {
                uint PosSlot = Encoder.GetPosSlot(i);
                int FooterBits = (int)((PosSlot >> 1) - 1);
                uint BaseVal = (2 | (PosSlot & 1)) << FooterBits;
                this.TempPrices[i] = BitTreeEncoder.ReverseGetPrice(this.PosEncoders, BaseVal - PosSlot - 1, FooterBits, i - BaseVal);
            }

            for (uint LenToPosState = 0; LenToPosState < Base.kNumLenToPosStates; LenToPosState++)
            {
                uint PosSlot;
                BitTreeEncoder encoder = this.PosSlotEncoder[LenToPosState];
                uint st = LenToPosState << Base.kNumPosSlotBits;
                for (PosSlot = 0; PosSlot < this.DistTableSize; PosSlot++)
                {
                    this.PosSlotPrices[st + PosSlot] = encoder.GetPrice(PosSlot);
                }

                for (PosSlot = Base.kEndPosModelIndex; PosSlot < this.DistTableSize; PosSlot++)
                {
                    this.PosSlotPrices[st + PosSlot] += ((PosSlot >> 1) - 1 - Base.kNumAlignBits) << BitEncoder.kNumBitPriceShiftBits;
                }

                uint st2 = LenToPosState * Base.kNumFullDistances;
                uint i;
                for (i = 0; i < Base.kStartPosModelIndex; i++)
                {
                    this.DistancesPrices[st2 + i] = this.PosSlotPrices[st + i];
                }

                for (; i < Base.kNumFullDistances; i++)
                {
                    this.DistancesPrices[st2 + i] = this.PosSlotPrices[st + Encoder.GetPosSlot(i)] + this.TempPrices[i];
                }
            }

            this.MatchPriceCount = 0;
        }

        private void Flush(uint NowPos)
        {
            this.ReleaseMfStream();
            this.WriteEndMarker(NowPos & this.PosStateMask);
            this.RangeEncoder.FlushData();
            this.RangeEncoder.FlushStream();
        }

        private uint GetOptimum(uint Position, out uint BackRes)
        {
            if (this.OptimumEndIndex != this.OptimumCurrentIndex)
            {
                uint LenRes = this.Optimum[this.OptimumCurrentIndex].PosPrev - this.OptimumCurrentIndex;
                BackRes = this.Optimum[this.OptimumCurrentIndex].BackPrev;
                this.OptimumCurrentIndex = this.Optimum[this.OptimumCurrentIndex].PosPrev;
                return LenRes;
            }

            this.OptimumCurrentIndex = this.OptimumEndIndex = 0;
            uint LenMain, NumDistancePairs;
            if (!this.LongestMatchWasFound)
            {
                this.ReadMatchDistances(out LenMain, out NumDistancePairs);
            }
            else
            {
                LenMain = this.LongestMatchLength;
                NumDistancePairs = this.NumDistancePairs;
                this.LongestMatchWasFound = false;
            }

            uint NumAvailableBytes = this.MatchFinder.GetNumAvailableBytes() + 1;
            if (NumAvailableBytes < 2)
            {
                BackRes = 0xFFFFFFFF;
                return 1;
            }

            if (NumAvailableBytes > Base.kMatchMaxLen)
            {
                NumAvailableBytes = Base.kMatchMaxLen;
            }

            uint RepMaxIndex = 0;
            uint i;
            for (i = 0; i < Base.kNumRepDistances; i++)
            {
                this.Reps[i] = this.RepDistances[i];
                this.RepLens[i] = this.MatchFinder.GetMatchLen(0 - 1, this.Reps[i], Base.kMatchMaxLen);
                if (this.RepLens[i] > this.RepLens[RepMaxIndex])
                {
                    RepMaxIndex = i;
                }
            }

            if (this.RepLens[RepMaxIndex] >= this.NumFastBytes)
            {
                BackRes = RepMaxIndex;
                uint LenRes = this.RepLens[RepMaxIndex];
                this.MovePos(LenRes - 1);
                return LenRes;
            }

            if (LenMain >= this.NumFastBytes)
            {
                BackRes = this.MatchDistances[NumDistancePairs - 1] + Base.kNumRepDistances;
                this.MovePos(LenMain - 1);
                return LenMain;
            }

            byte CurrentByte = this.MatchFinder.GetIndexByte(0 - 1);
            byte MatchByte = this.MatchFinder.GetIndexByte((Int32)(0 - this.RepDistances[0] - 1 - 1));
            if (LenMain < 2 && CurrentByte != MatchByte && this.RepLens[RepMaxIndex] < 2)
            {
                BackRes = 0xFFFFFFFF;
                return 1;
            }

            this.Optimum[0].State = this.State;
            uint PosState = Position & this.PosStateMask;
            this.Optimum[1].Price = this.IsMatch[(this.State.Index << Base.kNumPosStatesBitsMax) + PosState].GetPrice0() + this._literalEncoder.GetSubCoder(Position, this.Previoubyte).GetPrice(!this.State.IsCharState(), MatchByte, CurrentByte);
            this.Optimum[1].MakeAsChar();
            uint MatchPrice = this.IsMatch[(this.State.Index << Base.kNumPosStatesBitsMax) + PosState].GetPrice1();
            uint RepMatchPrice = MatchPrice + this.IsRep[this.State.Index].GetPrice1();
            if (MatchByte == CurrentByte)
            {
                uint ShortRepPrice = RepMatchPrice + this.GetRepLen1Price(this.State, PosState);
                if (ShortRepPrice < this.Optimum[1].Price)
                {
                    this.Optimum[1].Price = ShortRepPrice;
                    this.Optimum[1].MakeAsShortRep();
                }
            }

            uint LenEnd = LenMain >= this.RepLens[RepMaxIndex] ? LenMain : this.RepLens[RepMaxIndex];
            if (LenEnd < 2)
            {
                BackRes = this.Optimum[1].BackPrev;
                return 1;
            }

            this.Optimum[1].PosPrev = 0;
            this.Optimum[0].Backs0 = this.Reps[0];
            this.Optimum[0].Backs1 = this.Reps[1];
            this.Optimum[0].Backs2 = this.Reps[2];
            this.Optimum[0].Backs3 = this.Reps[3];
            uint len = LenEnd;
            do
            {
                this.Optimum[len--].Price = Encoder.kIfinityPrice;
            }
            while (len >= 2);
            for (i = 0; i < Base.kNumRepDistances; i++)
            {
                uint RepLen = this.RepLens[i];
                if (RepLen < 2)
                {
                    continue;
                }

                uint price = RepMatchPrice + this.GetPureRepPrice(i, this.State, PosState);
                do
                {
                    uint CurAndLenPrice = price + this.RepMatchLenEncoder.GetPrice(RepLen - 2, PosState);
                    Optimal optimum = this.Optimum[RepLen];
                    if (CurAndLenPrice < optimum.Price)
                    {
                        optimum.Price = CurAndLenPrice;
                        optimum.PosPrev = 0;
                        optimum.BackPrev = i;
                        optimum.Prev1IsChar = false;
                    }
                }
                while (--RepLen >= 2);
            }

            uint NormalMatchPrice = MatchPrice + this.IsRep[this.State.Index].GetPrice0();
            len = this.RepLens[0] >= 2 ? this.RepLens[0] + 1 : 2;
            if (len <= LenMain)
            {
                uint offs = 0;
                while (len > this.MatchDistances[offs])
                {
                    offs += 2;
                }

                for (;; len++)
                {
                    uint distance = this.MatchDistances[offs + 1];
                    uint CurAndLenPrice = NormalMatchPrice + this.GetPosLenPrice(distance, len, PosState);
                    Optimal optimum = this.Optimum[len];
                    if (CurAndLenPrice < optimum.Price)
                    {
                        optimum.Price = CurAndLenPrice;
                        optimum.PosPrev = 0;
                        optimum.BackPrev = distance + Base.kNumRepDistances;
                        optimum.Prev1IsChar = false;
                    }

                    if (len == this.MatchDistances[offs])
                    {
                        offs += 2;
                        if (offs == NumDistancePairs)
                        {
                            break;
                        }
                    }
                }
            }

            uint cur = 0;
            while (true)
            {
                cur++;
                if (cur == LenEnd)
                {
                    return this.Backward(out BackRes, cur);
                }

                uint NewLen;
                this.ReadMatchDistances(out NewLen, out NumDistancePairs);
                if (NewLen >= this.NumFastBytes)
                {
                    this.NumDistancePairs = NumDistancePairs;
                    this.LongestMatchLength = NewLen;
                    this.LongestMatchWasFound = true;
                    return this.Backward(out BackRes, cur);
                }

                Position++;
                uint PosPrev = this.Optimum[cur].PosPrev;
                Base.State state;
                if (this.Optimum[cur].Prev1IsChar)
                {
                    PosPrev--;
                    if (this.Optimum[cur].Prev2)
                    {
                        state = this.Optimum[this.Optimum[cur].PosPrev2].State;
                        if (this.Optimum[cur].BackPrev2 < Base.kNumRepDistances)
                        {
                            state.UpdateRep();
                        }
                        else
                        {
                            state.UpdateMatch();
                        }
                    }
                    else
                    {
                        state = this.Optimum[PosPrev].State;
                    }

                    state.UpdateChar();
                }
                else
                {
                    state = this.Optimum[PosPrev].State;
                }

                if (PosPrev == cur - 1)
                {
                    if (this.Optimum[cur].IsShortRep())
                    {
                        state.UpdateShortRep();
                    }
                    else
                    {
                        state.UpdateChar();
                    }
                }
                else
                {
                    uint pos;
                    if (this.Optimum[cur].Prev1IsChar && this.Optimum[cur].Prev2)
                    {
                        PosPrev = this.Optimum[cur].PosPrev2;
                        pos = this.Optimum[cur].BackPrev2;
                        state.UpdateRep();
                    }
                    else
                    {
                        pos = this.Optimum[cur].BackPrev;
                        if (pos < Base.kNumRepDistances)
                        {
                            state.UpdateRep();
                        }
                        else
                        {
                            state.UpdateMatch();
                        }
                    }

                    Optimal opt = this.Optimum[PosPrev];
                    if (pos < Base.kNumRepDistances)
                    {
                        if (pos == 0)
                        {
                            this.Reps[0] = opt.Backs0;
                            this.Reps[1] = opt.Backs1;
                            this.Reps[2] = opt.Backs2;
                            this.Reps[3] = opt.Backs3;
                        }
                        else if (pos == 1)
                        {
                            this.Reps[0] = opt.Backs1;
                            this.Reps[1] = opt.Backs0;
                            this.Reps[2] = opt.Backs2;
                            this.Reps[3] = opt.Backs3;
                        }
                        else if (pos == 2)
                        {
                            this.Reps[0] = opt.Backs2;
                            this.Reps[1] = opt.Backs0;
                            this.Reps[2] = opt.Backs1;
                            this.Reps[3] = opt.Backs3;
                        }
                        else
                        {
                            this.Reps[0] = opt.Backs3;
                            this.Reps[1] = opt.Backs0;
                            this.Reps[2] = opt.Backs1;
                            this.Reps[3] = opt.Backs2;
                        }
                    }
                    else
                    {
                        this.Reps[0] = pos - Base.kNumRepDistances;
                        this.Reps[1] = opt.Backs0;
                        this.Reps[2] = opt.Backs1;
                        this.Reps[3] = opt.Backs2;
                    }
                }

                this.Optimum[cur].State = state;
                this.Optimum[cur].Backs0 = this.Reps[0];
                this.Optimum[cur].Backs1 = this.Reps[1];
                this.Optimum[cur].Backs2 = this.Reps[2];
                this.Optimum[cur].Backs3 = this.Reps[3];
                uint CurPrice = this.Optimum[cur].Price;
                CurrentByte = this.MatchFinder.GetIndexByte(0 - 1);
                MatchByte = this.MatchFinder.GetIndexByte((Int32)(0 - this.Reps[0] - 1 - 1));
                PosState = Position & this.PosStateMask;
                uint CurAnd1Price = CurPrice + this.IsMatch[(state.Index << Base.kNumPosStatesBitsMax) + PosState].GetPrice0() + this._literalEncoder.GetSubCoder(Position, this.MatchFinder.GetIndexByte(0 - 2)).GetPrice(!state.IsCharState(), MatchByte, CurrentByte);
                Optimal NextOptimum = this.Optimum[cur + 1];
                bool NextIsChar = false;
                if (CurAnd1Price < NextOptimum.Price)
                {
                    NextOptimum.Price = CurAnd1Price;
                    NextOptimum.PosPrev = cur;
                    NextOptimum.MakeAsChar();
                    NextIsChar = true;
                }

                MatchPrice = CurPrice + this.IsMatch[(state.Index << Base.kNumPosStatesBitsMax) + PosState].GetPrice1();
                RepMatchPrice = MatchPrice + this.IsRep[state.Index].GetPrice1();
                if (MatchByte == CurrentByte && !(NextOptimum.PosPrev < cur && NextOptimum.BackPrev == 0))
                {
                    uint ShortRepPrice = RepMatchPrice + this.GetRepLen1Price(state, PosState);
                    if (ShortRepPrice <= NextOptimum.Price)
                    {
                        NextOptimum.Price = ShortRepPrice;
                        NextOptimum.PosPrev = cur;
                        NextOptimum.MakeAsShortRep();
                        NextIsChar = true;
                    }
                }

                uint NumAvailableBytesFull = this.MatchFinder.GetNumAvailableBytes() + 1;
                NumAvailableBytesFull = Math.Min(Encoder.kNumOpts - 1 - cur, NumAvailableBytesFull);
                NumAvailableBytes = NumAvailableBytesFull;
                if (NumAvailableBytes < 2)
                {
                    continue;
                }

                if (NumAvailableBytes > this.NumFastBytes)
                {
                    NumAvailableBytes = this.NumFastBytes;
                }

                if (!NextIsChar && MatchByte != CurrentByte)
                {
                    // try Literal + rep0
                    uint t = Math.Min(NumAvailableBytesFull - 1, this.NumFastBytes);
                    uint LenTest2 = this.MatchFinder.GetMatchLen(0, this.Reps[0], t);
                    if (LenTest2 >= 2)
                    {
                        Base.State state2 = state;
                        state2.UpdateChar();
                        uint PosStateNext = (Position + 1) & this.PosStateMask;
                        uint NextRepMatchPrice = CurAnd1Price + this.IsMatch[(state2.Index << Base.kNumPosStatesBitsMax) + PosStateNext].GetPrice1() + this.IsRep[state2.Index].GetPrice1();
                        {
                            uint offset = cur + 1 + LenTest2;
                            while (LenEnd < offset)
                            {
                                this.Optimum[++LenEnd].Price = Encoder.kIfinityPrice;
                            }

                            uint CurAndLenPrice = NextRepMatchPrice + this.GetRepPrice(0, LenTest2, state2, PosStateNext);
                            Optimal optimum = this.Optimum[offset];
                            if (CurAndLenPrice < optimum.Price)
                            {
                                optimum.Price = CurAndLenPrice;
                                optimum.PosPrev = cur + 1;
                                optimum.BackPrev = 0;
                                optimum.Prev1IsChar = true;
                                optimum.Prev2 = false;
                            }
                        }
                    }
                }

                uint StartLen = 2; // speed optimization
                for (uint RepIndex = 0; RepIndex < Base.kNumRepDistances; RepIndex++)
                {
                    uint LenTest = this.MatchFinder.GetMatchLen(0 - 1, this.Reps[RepIndex], NumAvailableBytes);
                    if (LenTest < 2)
                    {
                        continue;
                    }

                    uint LenTestTemp = LenTest;
                    do
                    {
                        while (LenEnd < cur + LenTest)
                        {
                            this.Optimum[++LenEnd].Price = Encoder.kIfinityPrice;
                        }

                        uint CurAndLenPrice = RepMatchPrice + this.GetRepPrice(RepIndex, LenTest, state, PosState);
                        Optimal optimum = this.Optimum[cur + LenTest];
                        if (CurAndLenPrice < optimum.Price)
                        {
                            optimum.Price = CurAndLenPrice;
                            optimum.PosPrev = cur;
                            optimum.BackPrev = RepIndex;
                            optimum.Prev1IsChar = false;
                        }
                    }
                    while (--LenTest >= 2);

                    LenTest = LenTestTemp;
                    if (RepIndex == 0)
                    {
                        StartLen = LenTest + 1;
                    }

                    // if (_maxMode)
                    if (LenTest < NumAvailableBytesFull)
                    {
                        uint t = Math.Min(NumAvailableBytesFull - 1 - LenTest, this.NumFastBytes);
                        uint LenTest2 = this.MatchFinder.GetMatchLen((Int32)LenTest, this.Reps[RepIndex], t);
                        if (LenTest2 >= 2)
                        {
                            Base.State state2 = state;
                            state2.UpdateRep();
                            uint PosStateNext = (Position + LenTest) & this.PosStateMask;
                            uint CurAndLenCharPrice = RepMatchPrice + this.GetRepPrice(RepIndex, LenTest, state, PosState) + this.IsMatch[(state2.Index << Base.kNumPosStatesBitsMax) + PosStateNext].GetPrice0() + this._literalEncoder.GetSubCoder(Position + LenTest, this.MatchFinder.GetIndexByte((Int32)LenTest - 1 - 1)).GetPrice(true, this.MatchFinder.GetIndexByte((Int32)LenTest - 1 - (Int32)(this.Reps[RepIndex] + 1)), this.MatchFinder.GetIndexByte((Int32)LenTest - 1));
                            state2.UpdateChar();
                            PosStateNext = (Position + LenTest + 1) & this.PosStateMask;
                            uint NextMatchPrice = CurAndLenCharPrice + this.IsMatch[(state2.Index << Base.kNumPosStatesBitsMax) + PosStateNext].GetPrice1();
                            uint NextRepMatchPrice = NextMatchPrice + this.IsRep[state2.Index].GetPrice1();
                            {
                                // for(; lenTest2 >= 2; lenTest2--)
                                uint offset = LenTest + 1 + LenTest2;
                                while (LenEnd < cur + offset)
                                {
                                    this.Optimum[++LenEnd].Price = Encoder.kIfinityPrice;
                                }

                                uint CurAndLenPrice = NextRepMatchPrice + this.GetRepPrice(0, LenTest2, state2, PosStateNext);
                                Optimal optimum = this.Optimum[cur + offset];
                                if (CurAndLenPrice < optimum.Price)
                                {
                                    optimum.Price = CurAndLenPrice;
                                    optimum.PosPrev = cur + LenTest + 1;
                                    optimum.BackPrev = 0;
                                    optimum.Prev1IsChar = true;
                                    optimum.Prev2 = true;
                                    optimum.PosPrev2 = cur;
                                    optimum.BackPrev2 = RepIndex;
                                }
                            }
                        }
                    }
                }

                if (NewLen > NumAvailableBytes)
                {
                    NewLen = NumAvailableBytes;
                    for (NumDistancePairs = 0; NewLen > this.MatchDistances[NumDistancePairs]; NumDistancePairs += 2)
                    {
                        ;
                    }

                    this.MatchDistances[NumDistancePairs] = NewLen;
                    NumDistancePairs += 2;
                }

                if (NewLen >= StartLen)
                {
                    NormalMatchPrice = MatchPrice + this.IsRep[state.Index].GetPrice0();
                    while (LenEnd < cur + NewLen)
                    {
                        this.Optimum[++LenEnd].Price = Encoder.kIfinityPrice;
                    }

                    uint offs = 0;
                    while (StartLen > this.MatchDistances[offs])
                    {
                        offs += 2;
                    }

                    for (uint LenTest = StartLen;; LenTest++)
                    {
                        uint CurBack = this.MatchDistances[offs + 1];
                        uint CurAndLenPrice = NormalMatchPrice + this.GetPosLenPrice(CurBack, LenTest, PosState);
                        Optimal optimum = this.Optimum[cur + LenTest];
                        if (CurAndLenPrice < optimum.Price)
                        {
                            optimum.Price = CurAndLenPrice;
                            optimum.PosPrev = cur;
                            optimum.BackPrev = CurBack + Base.kNumRepDistances;
                            optimum.Prev1IsChar = false;
                        }

                        if (LenTest == this.MatchDistances[offs])
                        {
                            if (LenTest < NumAvailableBytesFull)
                            {
                                uint t = Math.Min(NumAvailableBytesFull - 1 - LenTest, this.NumFastBytes);
                                uint LenTest2 = this.MatchFinder.GetMatchLen((Int32)LenTest, CurBack, t);
                                if (LenTest2 >= 2)
                                {
                                    Base.State state2 = state;
                                    state2.UpdateMatch();
                                    uint PosStateNext = (Position + LenTest) & this.PosStateMask;
                                    uint CurAndLenCharPrice = CurAndLenPrice + this.IsMatch[(state2.Index << Base.kNumPosStatesBitsMax) + PosStateNext].GetPrice0() + this._literalEncoder.GetSubCoder(Position + LenTest, this.MatchFinder.GetIndexByte((Int32)LenTest - 1 - 1)).GetPrice(true, this.MatchFinder.GetIndexByte((Int32)LenTest - (Int32)(CurBack + 1) - 1), this.MatchFinder.GetIndexByte((Int32)LenTest - 1));
                                    state2.UpdateChar();
                                    PosStateNext = (Position + LenTest + 1) & this.PosStateMask;
                                    uint NextMatchPrice = CurAndLenCharPrice + this.IsMatch[(state2.Index << Base.kNumPosStatesBitsMax) + PosStateNext].GetPrice1();
                                    uint NextRepMatchPrice = NextMatchPrice + this.IsRep[state2.Index].GetPrice1();
                                    uint offset = LenTest + 1 + LenTest2;
                                    while (LenEnd < cur + offset)
                                    {
                                        this.Optimum[++LenEnd].Price = Encoder.kIfinityPrice;
                                    }

                                    CurAndLenPrice = NextRepMatchPrice + this.GetRepPrice(0, LenTest2, state2, PosStateNext);
                                    optimum = this.Optimum[cur + offset];
                                    if (CurAndLenPrice < optimum.Price)
                                    {
                                        optimum.Price = CurAndLenPrice;
                                        optimum.PosPrev = cur + LenTest + 1;
                                        optimum.BackPrev = 0;
                                        optimum.Prev1IsChar = true;
                                        optimum.Prev2 = true;
                                        optimum.PosPrev2 = cur;
                                        optimum.BackPrev2 = CurBack + Base.kNumRepDistances;
                                    }
                                }
                            }

                            offs += 2;
                            if (offs == NumDistancePairs)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private uint GetPosLenPrice(uint Pos, uint Len, uint PosState)
        {
            uint price;
            uint LenToPosState = Base.GetLenToPosState(Len);
            if (Pos < Base.kNumFullDistances)
            {
                price = this.DistancesPrices[LenToPosState * Base.kNumFullDistances + Pos];
            }
            else
            {
                price = this.PosSlotPrices[(LenToPosState << Base.kNumPosSlotBits) + Encoder.GetPosSlot2(Pos)] + this.AlignPrices[Pos & Base.kAlignMask];
            }

            return price + this._lenEncoder.GetPrice(Len - Base.kMatchMinLen, PosState);
        }

        private uint GetPureRepPrice(uint RepIndex, Base.State State, uint PosState)
        {
            uint price;
            if (RepIndex == 0)
            {
                price = this.IsRepG0[State.Index].GetPrice0();
                price += this.IsRep0Long[(State.Index << Base.kNumPosStatesBitsMax) + PosState].GetPrice1();
            }
            else
            {
                price = this.IsRepG0[State.Index].GetPrice1();
                if (RepIndex == 1)
                {
                    price += this.IsRepG1[State.Index].GetPrice0();
                }
                else
                {
                    price += this.IsRepG1[State.Index].GetPrice1();
                    price += this.IsRepG2[State.Index].GetPrice(RepIndex - 2);
                }
            }

            return price;
        }

        private uint GetRepLen1Price(Base.State State, uint PosState)
        {
            return this.IsRepG0[State.Index].GetPrice0() + this.IsRep0Long[(State.Index << Base.kNumPosStatesBitsMax) + PosState].GetPrice0();
        }

        private uint GetRepPrice(uint RepIndex, uint Len, Base.State State, uint PosState)
        {
            uint price = this.RepMatchLenEncoder.GetPrice(Len - Base.kMatchMinLen, PosState);
            return price + this.GetPureRepPrice(RepIndex, State, PosState);
        }

        private void Init()
        {
            this.BaseInit();
            this.RangeEncoder.Init();
            uint i;
            for (i = 0; i < Base.kNumStates; i++)
            {
                for (uint j = 0; j <= this.PosStateMask; j++)
                {
                    uint ComplexState = (i << Base.kNumPosStatesBitsMax) + j;
                    this.IsMatch[ComplexState].Init();
                    this.IsRep0Long[ComplexState].Init();
                }

                this.IsRep[i].Init();
                this.IsRepG0[i].Init();
                this.IsRepG1[i].Init();
                this.IsRepG2[i].Init();
            }

            this._literalEncoder.Init();
            for (i = 0; i < Base.kNumLenToPosStates; i++)
            {
                this.PosSlotEncoder[i].Init();
            }

            for (i = 0; i < Base.kNumFullDistances - Base.kEndPosModelIndex; i++)
            {
                this.PosEncoders[i].Init();
            }

            this._lenEncoder.Init((UInt32)1 << this.PosStateBits);
            this.RepMatchLenEncoder.Init((UInt32)1 << this.PosStateBits);
            this.PosAlignEncoder.Init();
            this.LongestMatchWasFound = false;
            this.OptimumEndIndex = 0;
            this.OptimumCurrentIndex = 0;
            this.AdditionalOffset = 0;
        }

        private void MovePos(uint Num)
        {
            if (Num > 0)
            {
                this.MatchFinder.Skip(Num);
                this.AdditionalOffset += Num;
            }
        }

        private void ReadMatchDistances(out uint LenRes, out uint NumDistancePairs)
        {
            LenRes = 0;
            NumDistancePairs = this.MatchFinder.GetMatches(this.MatchDistances);
            if (NumDistancePairs > 0)
            {
                LenRes = this.MatchDistances[NumDistancePairs - 2];
                if (LenRes == this.NumFastBytes)
                {
                    LenRes += this.MatchFinder.GetMatchLen((int)LenRes - 1, this.MatchDistances[NumDistancePairs - 1], Base.kMatchMaxLen - LenRes);
                }
            }

            this.AdditionalOffset++;
        }

        private void ReleaseMfStream()
        {
            if (this.MatchFinder != null && this.NeedReleaseMfStream)
            {
                this.MatchFinder.ReleaseStream();
                this.NeedReleaseMfStream = false;
            }
        }

        private void ReleaseOutStream()
        {
            this.RangeEncoder.ReleaseStream();
        }

        private void ReleaseStreams()
        {
            this.ReleaseMfStream();
            this.ReleaseOutStream();
        }

        private void SetOutStream(Stream OutStream)
        {
            this.RangeEncoder.SetStream(OutStream);
        }

        private void SetStreams(Stream InStream, Stream OutStream, long InSize, long OutSize)
        {
            this.InStream = InStream;
            this.Finished = false;
            this.Create();
            this.SetOutStream(OutStream);
            this.Init();
            {
                // if (!_fastMode)
                this.FillDistancesPrices();
                this.FillAlignPrices();
            }

            this._lenEncoder.SetTableSize(this.NumFastBytes + 1 - Base.kMatchMinLen);
            this._lenEncoder.UpdateTables((UInt32)1 << this.PosStateBits);
            this.RepMatchLenEncoder.SetTableSize(this.NumFastBytes + 1 - Base.kMatchMinLen);
            this.RepMatchLenEncoder.UpdateTables((UInt32)1 << this.PosStateBits);
            this.NowPos64 = 0;
        }

        private void SetWriteEndMarkerMode(bool WriteEndMarker)
        {
            this.WriteEndMark = WriteEndMarker;
        }

        private void WriteEndMarker(uint PosState)
        {
            if (!this.WriteEndMark)
            {
                return;
            }

            this.IsMatch[(this.State.Index << Base.kNumPosStatesBitsMax) + PosState].Encode(this.RangeEncoder, 1);
            this.IsRep[this.State.Index].Encode(this.RangeEncoder, 0);
            this.State.UpdateMatch();
            uint len = Base.kMatchMinLen;
            this._lenEncoder.Encode(this.RangeEncoder, len - Base.kMatchMinLen, PosState);
            uint PosSlot = (1 << Base.kNumPosSlotBits) - 1;
            uint LenToPosState = Base.GetLenToPosState(len);
            this.PosSlotEncoder[LenToPosState].Encode(this.RangeEncoder, PosSlot);
            int FooterBits = 30;
            uint PosReduced = ((UInt32)1 << FooterBits) - 1;
            this.RangeEncoder.EncodeDirectBits(PosReduced >> Base.kNumAlignBits, FooterBits - Base.kNumAlignBits);
            this.PosAlignEncoder.ReverseEncode(this.RangeEncoder, PosReduced & Base.kAlignMask);
        }

        private class LenEncoder
        {
            private BitEncoder Choice = new BitEncoder();

            private BitEncoder Choice2 = new BitEncoder();

            private BitTreeEncoder HighCoder = new BitTreeEncoder(Base.kNumHighLenBits);

            private readonly BitTreeEncoder[] LowCoder = new BitTreeEncoder[Base.kNumPosStatesEncodingMax];

            private readonly BitTreeEncoder[] MidCoder = new BitTreeEncoder[Base.kNumPosStatesEncodingMax];

            public LenEncoder()
            {
                for (uint PosState = 0; PosState < Base.kNumPosStatesEncodingMax; PosState++)
                {
                    this.LowCoder[PosState] = new BitTreeEncoder(Base.kNumLowLenBits);
                    this.MidCoder[PosState] = new BitTreeEncoder(Base.kNumMidLenBits);
                }
            }

            public void Encode(RangeCoder.Encoder RangeEncoder, uint Symbol, uint PosState)
            {
                if (Symbol < Base.kNumLowLenSymbols)
                {
                    this.Choice.Encode(RangeEncoder, 0);
                    this.LowCoder[PosState].Encode(RangeEncoder, Symbol);
                }
                else
                {
                    Symbol -= Base.kNumLowLenSymbols;
                    this.Choice.Encode(RangeEncoder, 1);
                    if (Symbol < Base.kNumMidLenSymbols)
                    {
                        this.Choice2.Encode(RangeEncoder, 0);
                        this.MidCoder[PosState].Encode(RangeEncoder, Symbol);
                    }
                    else
                    {
                        this.Choice2.Encode(RangeEncoder, 1);
                        this.HighCoder.Encode(RangeEncoder, Symbol - Base.kNumMidLenSymbols);
                    }
                }
            }

            public void Init(uint NumPosStates)
            {
                this.Choice.Init();
                this.Choice2.Init();
                for (uint PosState = 0; PosState < NumPosStates; PosState++)
                {
                    this.LowCoder[PosState].Init();
                    this.MidCoder[PosState].Init();
                }

                this.HighCoder.Init();
            }

            public void SetPrices(uint PosState, uint NumSymbols, uint[] Prices, uint St)
            {
                uint a0 = this.Choice.GetPrice0();
                uint a1 = this.Choice.GetPrice1();
                uint b0 = a1 + this.Choice2.GetPrice0();
                uint b1 = a1 + this.Choice2.GetPrice1();
                uint i = 0;
                for (i = 0; i < Base.kNumLowLenSymbols; i++)
                {
                    if (i >= NumSymbols)
                    {
                        return;
                    }

                    Prices[St + i] = a0 + this.LowCoder[PosState].GetPrice(i);
                }

                for (; i < Base.kNumLowLenSymbols + Base.kNumMidLenSymbols; i++)
                {
                    if (i >= NumSymbols)
                    {
                        return;
                    }

                    Prices[St + i] = b0 + this.MidCoder[PosState].GetPrice(i - Base.kNumLowLenSymbols);
                }

                for (; i < NumSymbols; i++)
                {
                    Prices[St + i] = b1 + this.HighCoder.GetPrice(i - Base.kNumLowLenSymbols - Base.kNumMidLenSymbols);
                }
            }
        }

        private class LenPriceTableEncoder : LenEncoder
        {
            private readonly uint[] Counters = new uint[Base.kNumPosStatesEncodingMax];

            private readonly uint[] Prices = new uint[Base.kNumLenSymbols << Base.kNumPosStatesBitsEncodingMax];

            private uint TableSize;

            public new void Encode(RangeCoder.Encoder RangeEncoder, uint Symbol, uint PosState)
            {
                base.Encode(RangeEncoder, Symbol, PosState);
                if (--this.Counters[PosState] == 0)
                {
                    this.UpdateTable(PosState);
                }
            }

            public uint GetPrice(uint Symbol, uint PosState)
            {
                return this.Prices[PosState * Base.kNumLenSymbols + Symbol];
            }

            public void SetTableSize(uint TableSize)
            {
                this.TableSize = TableSize;
            }

            public void UpdateTables(uint NumPosStates)
            {
                for (uint PosState = 0; PosState < NumPosStates; PosState++)
                {
                    this.UpdateTable(PosState);
                }
            }

            private void UpdateTable(uint PosState)
            {
                this.SetPrices(PosState, this.TableSize, this.Prices, PosState * Base.kNumLenSymbols);
                this.Counters[PosState] = this.TableSize;
            }
        }

        private class LiteralEncoder
        {
            private Encoder2[] MCoders;

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
                this.MCoders = new Encoder2[NumStates];
                for (uint i = 0; i < NumStates; i++)
                {
                    this.MCoders[i].Create();
                }
            }

            public Encoder2 GetSubCoder(uint Pos, byte PrevByte)
            {
                return this.MCoders[((Pos & this.MPosMask) << this.MNumPrevBits) + (uint)(PrevByte >> (8 - this.MNumPrevBits))];
            }

            public void Init()
            {
                uint NumStates = (uint)1 << (this.MNumPrevBits + this.MNumPosBits);
                for (uint i = 0; i < NumStates; i++)
                {
                    this.MCoders[i].Init();
                }
            }

            public struct Encoder2
            {
                private BitEncoder[] MEncoders;

                public void Create()
                {
                    this.MEncoders = new BitEncoder[0x300];
                }

                public void Init()
                {
                    for (int i = 0; i < 0x300; i++)
                    {
                        this.MEncoders[i].Init();
                    }
                }

                public void Encode(RangeCoder.Encoder RangeEncoder, byte Symbol)
                {
                    uint context = 1;
                    for (int i = 7; i >= 0; i--)
                    {
                        uint bit = (uint)((Symbol >> i) & 1);
                        this.MEncoders[context].Encode(RangeEncoder, bit);
                        context = (context << 1) | bit;
                    }
                }

                public void EncodeMatched(RangeCoder.Encoder RangeEncoder, byte MatchByte, byte Symbol)
                {
                    uint context = 1;
                    bool same = true;
                    for (int i = 7; i >= 0; i--)
                    {
                        uint bit = (uint)((Symbol >> i) & 1);
                        uint state = context;
                        if (same)
                        {
                            uint MatchBit = (uint)((MatchByte >> i) & 1);
                            state += (1 + MatchBit) << 8;
                            same = MatchBit == bit;
                        }

                        this.MEncoders[state].Encode(RangeEncoder, bit);
                        context = (context << 1) | bit;
                    }
                }

                public uint GetPrice(bool MatchMode, byte MatchByte, byte Symbol)
                {
                    uint price = 0;
                    uint context = 1;
                    int i = 7;
                    if (MatchMode)
                    {
                        for (; i >= 0; i--)
                        {
                            uint MatchBit = (uint)(MatchByte >> i) & 1;
                            uint bit = (uint)(Symbol >> i) & 1;
                            price += this.MEncoders[((1 + MatchBit) << 8) + context].GetPrice(bit);
                            context = (context << 1) | bit;
                            if (MatchBit != bit)
                            {
                                i--;
                                break;
                            }
                        }
                    }

                    for (; i >= 0; i--)
                    {
                        uint bit = (uint)(Symbol >> i) & 1;
                        price += this.MEncoders[context].GetPrice(bit);
                        context = (context << 1) | bit;
                    }

                    return price;
                }
            }
        }

        private class Optimal
        {
            public uint BackPrev;

            public uint BackPrev2;

            public uint Backs0;

            public uint Backs1;

            public uint Backs2;

            public uint Backs3;

            public uint PosPrev;

            public uint PosPrev2;

            public bool Prev1IsChar;

            public bool Prev2;

            public uint Price;

            public Base.State State;

            public bool IsShortRep()
            {
                return this.BackPrev == 0;
            }

            public void MakeAsChar()
            {
                this.BackPrev = 0xFFFFFFFF;
                this.Prev1IsChar = false;
            }

            public void MakeAsShortRep()
            {
                this.BackPrev = 0;
                this.Prev1IsChar = false;
            }
        }
    }
}