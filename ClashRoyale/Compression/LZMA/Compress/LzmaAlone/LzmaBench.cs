namespace ClashRoyale.Compression.LZMA.Compress.LzmaAlone
{
    using System;
    using System.IO;

    using ClashRoyale.Compression.LZMA.Common;
    using ClashRoyale.Compression.LZMA.Compress.LZMA;

    /// <summary>
    /// LZMA Benchmark
    /// </summary>
    internal abstract class LzmaBench
    {
        private const uint kAdditionalSize = 6 << 20;

        private const uint kCompressedAdditionalSize = 1 << 10;

        private const uint kMaxLzmaPropSize = 10;

        private const int kSubBits = 8;

        public static int LzmaBenchmark(int NumIterations, uint DictionarySize)
        {
            if (NumIterations <= 0)
            {
                return 0;
            }

            if (DictionarySize < 1 << 18)
            {
                Console.WriteLine("\nError: dictionary size for benchmark must be >= 19 (512 KB)");
                return 1;
            }

            Console.Write("\n       Compressing                Decompressing\n\n");
            Encoder encoder = new Encoder();
            Decoder decoder = new Decoder();
            CoderPropId[] PropIDs =
                {
                    CoderPropId.DictionarySize
                };
            object[] properties =
                {
                    (Int32)DictionarySize
                };
            uint KBufferSize = DictionarySize + LzmaBench.kAdditionalSize;
            uint KCompressedBufferSize = KBufferSize / 2 + LzmaBench.kCompressedAdditionalSize;
            encoder.SetCoderProperties(PropIDs, properties);
            MemoryStream PropStream = new MemoryStream();
            encoder.WriteCoderProperties(PropStream);
            byte[] PropArray = PropStream.ToArray();
            CBenchRandomGenerator rg = new CBenchRandomGenerator();
            rg.Set(KBufferSize);
            rg.Generate();
            Crc crc = new Crc();
            crc.Init();
            crc.Update(rg.Buffer, 0, rg.BufferSize);
            CProgressInfo ProgressInfo = new CProgressInfo();
            ProgressInfo.ApprovedStart = DictionarySize;
            ulong TotalBenchSize = 0;
            ulong TotalEncodeTime = 0;
            ulong TotalDecodeTime = 0;
            ulong TotalCompressedSize = 0;
            MemoryStream InStream = new MemoryStream(rg.Buffer, 0, (int)rg.BufferSize);
            MemoryStream CompressedStream = new MemoryStream((int)KCompressedBufferSize);
            CrcOutStream CrcOutStream = new CrcOutStream();
            for (int i = 0; i < NumIterations; i++)
            {
                ProgressInfo.Init();
                InStream.Seek(0, SeekOrigin.Begin);
                CompressedStream.Seek(0, SeekOrigin.Begin);
                encoder.Code(InStream, CompressedStream, -1, -1, ProgressInfo);
                TimeSpan sp2 = DateTime.UtcNow - ProgressInfo.Time;
                ulong EncodeTime = (UInt64)sp2.Ticks;
                long CompressedSize = CompressedStream.Position;
                if (ProgressInfo.InSize == 0)
                {
                    throw new Exception("Internal ERROR 1282");
                }

                ulong DecodeTime = 0;
                for (int j = 0; j < 2; j++)
                {
                    CompressedStream.Seek(0, SeekOrigin.Begin);
                    CrcOutStream.Init();
                    decoder.SetDecoderProperties(PropArray);
                    ulong OutSize = KBufferSize;
                    DateTime StartTime = DateTime.UtcNow;
                    decoder.Code(CompressedStream, CrcOutStream, 0, (Int64)OutSize, null);
                    TimeSpan sp = DateTime.UtcNow - StartTime;
                    DecodeTime = (ulong)sp.Ticks;
                    if (CrcOutStream.GetDigest() != crc.GetDigest())
                    {
                        throw new Exception("CRC Error");
                    }
                }

                ulong BenchSize = KBufferSize - (UInt64)ProgressInfo.InSize;
                LzmaBench.PrintResults(DictionarySize, EncodeTime, BenchSize, false, 0);
                Console.Write("     ");
                LzmaBench.PrintResults(DictionarySize, DecodeTime, KBufferSize, true, (ulong)CompressedSize);
                Console.WriteLine();
                TotalBenchSize += BenchSize;
                TotalEncodeTime += EncodeTime;
                TotalDecodeTime += DecodeTime;
                TotalCompressedSize += (ulong)CompressedSize;
            }

            Console.WriteLine("---------------------------------------------------");
            LzmaBench.PrintResults(DictionarySize, TotalEncodeTime, TotalBenchSize, false, 0);
            Console.Write("     ");
            LzmaBench.PrintResults(DictionarySize, TotalDecodeTime, KBufferSize * (UInt64)NumIterations, true, TotalCompressedSize);
            Console.WriteLine("    Average");
            return 0;
        }

        private static ulong GetCompressRating(uint DictionarySize, ulong ElapsedTime, ulong Size)
        {
            ulong t = LzmaBench.GetLogSize(DictionarySize) - (18 << LzmaBench.kSubBits);
            ulong NumCommandsForOne = 1060 + ((t * t * 10) >> (2 * LzmaBench.kSubBits));
            ulong NumCommands = Size * NumCommandsForOne;
            return LzmaBench.MyMultDiv64(NumCommands, ElapsedTime);
        }

        private static ulong GetDecompressRating(ulong ElapsedTime, ulong OutSize, ulong InSize)
        {
            ulong NumCommands = InSize * 220 + OutSize * 20;
            return LzmaBench.MyMultDiv64(NumCommands, ElapsedTime);
        }

        private static uint GetLogSize(uint Size)
        {
            for (int i = LzmaBench.kSubBits; i < 32; i++)
            {
                for (uint j = 0; j < 1 << LzmaBench.kSubBits; j++)
                {
                    if (Size <= ((UInt32)1 << i) + (j << (i - LzmaBench.kSubBits)))
                    {
                        return (UInt32)(i << LzmaBench.kSubBits) + j;
                    }
                }
            }

            return 32 << LzmaBench.kSubBits;
        }

        private static ulong GetTotalRating(uint DictionarySize, ulong ElapsedTimeEn, ulong SizeEn, ulong ElapsedTimeDe, ulong InSizeDe, ulong OutSizeDe)
        {
            return (LzmaBench.GetCompressRating(DictionarySize, ElapsedTimeEn, SizeEn) + LzmaBench.GetDecompressRating(ElapsedTimeDe, InSizeDe, OutSizeDe)) / 2;
        }

        private static ulong MyMultDiv64(ulong Value, ulong ElapsedTime)
        {
            ulong freq = TimeSpan.TicksPerSecond;
            ulong ElTime = ElapsedTime;
            while (freq > 1000000)
            {
                freq >>= 1;
                ElTime >>= 1;
            }

            if (ElTime == 0)
            {
                ElTime = 1;
            }

            return Value * freq / ElTime;
        }

        private static void PrintRating(ulong Rating)
        {
            LzmaBench.PrintValue(Rating / 1000000);
            Console.Write(" MIPS");
        }

        private static void PrintResults(uint DictionarySize, ulong ElapsedTime, ulong Size, bool DecompressMode, ulong SecondSize)
        {
            ulong speed = LzmaBench.MyMultDiv64(Size, ElapsedTime);
            LzmaBench.PrintValue(speed / 1024);
            Console.Write(" KB/s  ");
            ulong rating;
            if (DecompressMode)
            {
                rating = LzmaBench.GetDecompressRating(ElapsedTime, Size, SecondSize);
            }
            else
            {
                rating = LzmaBench.GetCompressRating(DictionarySize, ElapsedTime, Size);
            }

            LzmaBench.PrintRating(rating);
        }

        private static void PrintValue(ulong V)
        {
            string s = V.ToString();
            for (int i = 0; i + s.Length < 6; i++)
            {
                Console.Write(" ");
            }

            Console.Write(s);
        }

        private class CBenchRandomGenerator
        {
            public byte[] Buffer;

            public uint BufferSize;

            private uint Pos;

            private uint Rep0;

            private readonly CBitRandomGenerator Rg = new CBitRandomGenerator();

            public void Generate()
            {
                this.Rg.Init();
                this.Rep0 = 1;
                while (this.Pos < this.BufferSize)
                {
                    if (this.GetRndBit() == 0 || this.Pos < 1)
                    {
                        this.Buffer[this.Pos++] = (Byte)this.Rg.GetRnd(8);
                    }
                    else
                    {
                        uint len;
                        if (this.Rg.GetRnd(3) == 0)
                        {
                            len = 1 + this.GetLen1();
                        }
                        else
                        {
                            do
                            {
                                this.Rep0 = this.GetOffset();
                            }
                            while (this.Rep0 >= this.Pos);

                            this.Rep0++;
                            len = 2 + this.GetLen2();
                        }

                        for (uint i = 0; i < len && this.Pos < this.BufferSize; i++, this.Pos++)
                        {
                            this.Buffer[this.Pos] = this.Buffer[this.Pos - this.Rep0];
                        }
                    }
                }
            }

            public void Set(uint BufferSize)
            {
                this.Buffer = new byte[BufferSize];
                this.Pos = 0;
                this.BufferSize = BufferSize;
            }

            private uint GetLen1()
            {
                return this.Rg.GetRnd(1 + (int)this.Rg.GetRnd(2));
            }

            private uint GetLen2()
            {
                return this.Rg.GetRnd(2 + (int)this.Rg.GetRnd(2));
            }

            private uint GetLogRandBits(int NumBits)
            {
                uint len = this.Rg.GetRnd(NumBits);
                return this.Rg.GetRnd((int)len);
            }

            private uint GetOffset()
            {
                if (this.GetRndBit() == 0)
                {
                    return this.GetLogRandBits(4);
                }

                return (this.GetLogRandBits(4) << 10) | this.Rg.GetRnd(10);
            }

            private uint GetRndBit()
            {
                return this.Rg.GetRnd(1);
            }
        }

        private class CBitRandomGenerator
        {
            private int NumBits;

            private readonly CRandomGenerator Rg = new CRandomGenerator();

            private uint Value;

            public uint GetRnd(int NumBits)
            {
                uint result;
                if (this.NumBits > NumBits)
                {
                    result = this.Value & (((UInt32)1 << NumBits) - 1);
                    this.Value >>= NumBits;
                    this.NumBits -= NumBits;
                    return result;
                }

                NumBits -= this.NumBits;
                result = this.Value << NumBits;
                this.Value = this.Rg.GetRnd();
                result |= this.Value & (((UInt32)1 << NumBits) - 1);
                this.Value >>= NumBits;
                this.NumBits = 32 - NumBits;
                return result;
            }

            public void Init()
            {
                this.Value = 0;
                this.NumBits = 0;
            }
        }

        private class CProgressInfo : ICodeProgress
        {
            public long ApprovedStart;

            public long InSize;

            public DateTime Time;

            public void Init()
            {
                this.InSize = 0;
            }

            public void SetProgress(long InSize, long OutSize)
            {
                if (InSize >= this.ApprovedStart && this.InSize == 0)
                {
                    this.Time = DateTime.UtcNow;
                    this.InSize = InSize;
                }
            }
        }

        private class CRandomGenerator
        {
            private uint A1;

            private uint A2;

            public CRandomGenerator()
            {
                this.Init();
            }

            public uint GetRnd()
            {
                return ((this.A1 = 36969 * (this.A1 & 0xffff) + (this.A1 >> 16)) << 16) ^ (this.A2 = 18000 * (this.A2 & 0xffff) + (this.A2 >> 16));
            }

            public void Init()
            {
                this.A1 = 362436069;
                this.A2 = 521288629;
            }
        }

        private class CrcOutStream : Stream
        {
            public readonly Crc Crc = new Crc();

            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => 0;

            public override long Position
            {
                get
                {
                    return 0;
                }

                set
                {
                }
            }

            public override void Flush()
            {
            }

            public uint GetDigest()
            {
                return this.Crc.GetDigest();
            }

            public void Init()
            {
                this.Crc.Init();
            }

            public override int Read(byte[] Buffer, int Offset, int Count)
            {
                return 0;
            }

            public override long Seek(long Offset, SeekOrigin Origin)
            {
                return 0;
            }

            public override void SetLength(long Value)
            {
            }

            public override void Write(byte[] Buffer, int Offset, int Count)
            {
                this.Crc.Update(Buffer, (uint)Offset, (uint)Count);
            }

            public override void WriteByte(byte B)
            {
                this.Crc.UpdateByte(B);
            }
        }
    }
}