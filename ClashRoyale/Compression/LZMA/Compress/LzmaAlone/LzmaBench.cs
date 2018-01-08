namespace ClashRoyale.Compression.Lzma.Compress.LzmaAlone
{
    using System;
    using System.IO;

    using ClashRoyale.Compression.Lzma.Common;
    using ClashRoyale.Compression.Lzma.Compress.LZMA;

    /// <summary>
    /// LZMA Benchmark
    /// </summary>
    internal abstract class LzmaBench
    {
        private const uint kAdditionalSize = 6 << 20;

        private const uint kCompressedAdditionalSize = 1 << 10;

        private const uint kMaxLzmaPropSize = 10;

        private const int kSubBits = 8;

        public static int LzmaBenchmark(int numIterations, uint dictionarySize)
        {
            if (numIterations <= 0)
            {
                return 0;
            }

            if (dictionarySize < 1 << 18)
            {
                Console.WriteLine("\nError: dictionary size for benchmark must be >= 19 (512 KB)");
                return 1;
            }

            Console.Write("\n       Compressing                Decompressing\n\n");
            Encoder encoder = new Encoder();
            Decoder decoder = new Decoder();
            CoderPropId[] propIDs =
                {
                    CoderPropId.DictionarySize
                };
            object[] properties =
                {
                    (Int32)dictionarySize
                };
            uint kBufferSize = dictionarySize + LzmaBench.kAdditionalSize;
            uint kCompressedBufferSize = kBufferSize / 2 + LzmaBench.kCompressedAdditionalSize;
            encoder.SetCoderProperties(propIDs, properties);
            MemoryStream propStream = new MemoryStream();
            encoder.WriteCoderProperties(propStream);
            byte[] propArray = propStream.ToArray();
            CBenchRandomGenerator rg = new CBenchRandomGenerator();
            rg.Set(kBufferSize);
            rg.Generate();
            CRC crc = new CRC();
            crc.Init();
            crc.Update(rg.Buffer, 0, rg.BufferSize);
            CProgressInfo progressInfo = new CProgressInfo();
            progressInfo.ApprovedStart = dictionarySize;
            ulong totalBenchSize = 0;
            ulong totalEncodeTime = 0;
            ulong totalDecodeTime = 0;
            ulong totalCompressedSize = 0;
            MemoryStream inStream = new MemoryStream(rg.Buffer, 0, (int)rg.BufferSize);
            MemoryStream compressedStream = new MemoryStream((int)kCompressedBufferSize);
            CrcOutStream crcOutStream = new CrcOutStream();
            for (int i = 0; i < numIterations; i++)
            {
                progressInfo.Init();
                inStream.Seek(0, SeekOrigin.Begin);
                compressedStream.Seek(0, SeekOrigin.Begin);
                encoder.Code(inStream, compressedStream, -1, -1, progressInfo);
                TimeSpan sp2 = DateTime.UtcNow - progressInfo.Time;
                ulong encodeTime = (UInt64)sp2.Ticks;
                long compressedSize = compressedStream.Position;
                if (progressInfo.InSize == 0)
                {
                    throw new Exception("Internal ERROR 1282");
                }

                ulong decodeTime = 0;
                for (int j = 0; j < 2; j++)
                {
                    compressedStream.Seek(0, SeekOrigin.Begin);
                    crcOutStream.Init();
                    decoder.SetDecoderProperties(propArray);
                    ulong outSize = kBufferSize;
                    DateTime startTime = DateTime.UtcNow;
                    decoder.Code(compressedStream, crcOutStream, 0, (Int64)outSize, null);
                    TimeSpan sp = DateTime.UtcNow - startTime;
                    decodeTime = (ulong)sp.Ticks;
                    if (crcOutStream.GetDigest() != crc.GetDigest())
                    {
                        throw new Exception("CRC Error");
                    }
                }

                ulong benchSize = kBufferSize - (UInt64)progressInfo.InSize;
                LzmaBench.PrintResults(dictionarySize, encodeTime, benchSize, false, 0);
                Console.Write("     ");
                LzmaBench.PrintResults(dictionarySize, decodeTime, kBufferSize, true, (ulong)compressedSize);
                Console.WriteLine();
                totalBenchSize += benchSize;
                totalEncodeTime += encodeTime;
                totalDecodeTime += decodeTime;
                totalCompressedSize += (ulong)compressedSize;
            }

            Console.WriteLine("---------------------------------------------------");
            LzmaBench.PrintResults(dictionarySize, totalEncodeTime, totalBenchSize, false, 0);
            Console.Write("     ");
            LzmaBench.PrintResults(dictionarySize, totalDecodeTime, kBufferSize * (UInt64)numIterations, true, totalCompressedSize);
            Console.WriteLine("    Average");
            return 0;
        }

        private static ulong GetCompressRating(uint dictionarySize, ulong elapsedTime, ulong size)
        {
            ulong t = LzmaBench.GetLogSize(dictionarySize) - (18 << LzmaBench.kSubBits);
            ulong numCommandsForOne = 1060 + ((t * t * 10) >> (2 * LzmaBench.kSubBits));
            ulong numCommands = size * numCommandsForOne;
            return LzmaBench.MyMultDiv64(numCommands, elapsedTime);
        }

        private static ulong GetDecompressRating(ulong elapsedTime, ulong outSize, ulong inSize)
        {
            ulong numCommands = inSize * 220 + outSize * 20;
            return LzmaBench.MyMultDiv64(numCommands, elapsedTime);
        }

        private static uint GetLogSize(uint size)
        {
            for (int i = LzmaBench.kSubBits; i < 32; i++)
            {
                for (uint j = 0; j < 1 << LzmaBench.kSubBits; j++)
                {
                    if (size <= ((UInt32)1 << i) + (j << (i - LzmaBench.kSubBits)))
                    {
                        return (UInt32)(i << LzmaBench.kSubBits) + j;
                    }
                }
            }

            return 32 << LzmaBench.kSubBits;
        }

        private static ulong GetTotalRating(uint dictionarySize, ulong elapsedTimeEn, ulong sizeEn, ulong elapsedTimeDe, ulong inSizeDe, ulong outSizeDe)
        {
            return (LzmaBench.GetCompressRating(dictionarySize, elapsedTimeEn, sizeEn) + LzmaBench.GetDecompressRating(elapsedTimeDe, inSizeDe, outSizeDe)) / 2;
        }

        private static ulong MyMultDiv64(ulong value, ulong elapsedTime)
        {
            ulong freq = TimeSpan.TicksPerSecond;
            ulong elTime = elapsedTime;
            while (freq > 1000000)
            {
                freq >>= 1;
                elTime >>= 1;
            }

            if (elTime == 0)
            {
                elTime = 1;
            }

            return value * freq / elTime;
        }

        private static void PrintRating(ulong rating)
        {
            LzmaBench.PrintValue(rating / 1000000);
            Console.Write(" MIPS");
        }

        private static void PrintResults(uint dictionarySize, ulong elapsedTime, ulong size, bool decompressMode, ulong secondSize)
        {
            ulong speed = LzmaBench.MyMultDiv64(size, elapsedTime);
            LzmaBench.PrintValue(speed / 1024);
            Console.Write(" KB/s  ");
            ulong rating;
            if (decompressMode)
            {
                rating = LzmaBench.GetDecompressRating(elapsedTime, size, secondSize);
            }
            else
            {
                rating = LzmaBench.GetCompressRating(dictionarySize, elapsedTime, size);
            }

            LzmaBench.PrintRating(rating);
        }

        private static void PrintValue(ulong v)
        {
            string s = v.ToString();
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

            private readonly CBitRandomGenerator RG = new CBitRandomGenerator();

            public void Generate()
            {
                this.RG.Init();
                this.Rep0 = 1;
                while (this.Pos < this.BufferSize)
                {
                    if (this.GetRndBit() == 0 || this.Pos < 1)
                    {
                        this.Buffer[this.Pos++] = (Byte)this.RG.GetRnd(8);
                    }
                    else
                    {
                        uint len;
                        if (this.RG.GetRnd(3) == 0)
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

            public void Set(uint bufferSize)
            {
                this.Buffer = new byte[bufferSize];
                this.Pos = 0;
                this.BufferSize = bufferSize;
            }

            private uint GetLen1()
            {
                return this.RG.GetRnd(1 + (int)this.RG.GetRnd(2));
            }

            private uint GetLen2()
            {
                return this.RG.GetRnd(2 + (int)this.RG.GetRnd(2));
            }

            private uint GetLogRandBits(int numBits)
            {
                uint len = this.RG.GetRnd(numBits);
                return this.RG.GetRnd((int)len);
            }

            private uint GetOffset()
            {
                if (this.GetRndBit() == 0)
                {
                    return this.GetLogRandBits(4);
                }

                return (this.GetLogRandBits(4) << 10) | this.RG.GetRnd(10);
            }

            private uint GetRndBit()
            {
                return this.RG.GetRnd(1);
            }
        }

        private class CBitRandomGenerator
        {
            private int NumBits;

            private readonly CRandomGenerator RG = new CRandomGenerator();

            private uint Value;

            public uint GetRnd(int numBits)
            {
                uint result;
                if (this.NumBits > numBits)
                {
                    result = this.Value & (((UInt32)1 << numBits) - 1);
                    this.Value >>= numBits;
                    this.NumBits -= numBits;
                    return result;
                }

                numBits -= this.NumBits;
                result = this.Value << numBits;
                this.Value = this.RG.GetRnd();
                result |= this.Value & (((UInt32)1 << numBits) - 1);
                this.Value >>= numBits;
                this.NumBits = 32 - numBits;
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

            public void SetProgress(long inSize, long outSize)
            {
                if (inSize >= this.ApprovedStart && this.InSize == 0)
                {
                    this.Time = DateTime.UtcNow;
                    this.InSize = inSize;
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
            public readonly CRC CRC = new CRC();

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
                return this.CRC.GetDigest();
            }

            public void Init()
            {
                this.CRC.Init();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return 0;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return 0;
            }

            public override void SetLength(long value)
            {
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                this.CRC.Update(buffer, (uint)offset, (uint)count);
            }

            public override void WriteByte(byte b)
            {
                this.CRC.UpdateByte(b);
            }
        }
    }
}