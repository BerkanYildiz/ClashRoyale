namespace ClashRoyale.Compression.LZMA.Compress.LzmaAlone
{
    using System;
    using System.Collections;
    using System.IO;

    using ClashRoyale.Compression.LZMA.Common;
    using ClashRoyale.Compression.LZMA.Compress.LZMA;

    public class CDoubleStream : Stream
    {
        public int FileIndex;

        public Stream S1;

        public Stream S2;

        public long SkipSize;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => this.S1.Length + this.S2.Length - this.SkipSize;

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

        public override int Read(byte[] Buffer, int Offset, int Count)
        {
            int NumTotal = 0;
            while (Count > 0)
            {
                if (this.FileIndex == 0)
                {
                    int num = this.S1.Read(Buffer, Offset, Count);
                    Offset += num;
                    Count -= num;
                    NumTotal += num;
                    if (num == 0)
                    {
                        this.FileIndex++;
                    }
                }

                if (this.FileIndex == 1)
                {
                    NumTotal += this.S2.Read(Buffer, Offset, Count);
                    return NumTotal;
                }
            }

            return NumTotal;
        }

        public override long Seek(long Offset, SeekOrigin Origin)
        {
            throw new Exception("can't Seek");
        }

        public override void SetLength(long Value)
        {
            throw new Exception("can't SetLength");
        }

        public override void Write(byte[] Buffer, int Offset, int Count)
        {
            throw new Exception("can't Write");
        }
    }

    public class LzmaAlone
    {
        private enum Key
        {
            Help1 = 0,

            Help2,

            Mode,

            Dictionary,

            FastBytes,

            LitContext,

            LitPos,

            PosBits,

            MatchFinder,

            Eos,

            StdIn,

            StdOut,

            Train
        }

        private static bool GetNumber(string S, out int V)
        {
            V = 0;
            for (int i = 0; i < S.Length; i++)
            {
                char c = S[i];
                if (c < '0' || c > '9')
                {
                    return false;
                }

                V *= 10;
                V += c - '0';
            }

            return true;
        }

        private static int IncorrectCommand()
        {
            throw new Exception("Command line error");

            // System.Console.WriteLine("\nCommand line error\n"); return 1;
        }

        private static void PrintHelp()
        {
            Console.WriteLine(
                "\nUsage:  LZMA <e|d> [<switches>...] inputFile outputFile\n" + "  e: encode file\n" + "  d: decode file\n" + "  b: Benchmark\n" + "<Switches>\n" +

                // " -a{N}: set compression mode - [0, 1], default: 1 (max)\n" +
                "  -d{N}:  set dictionary - [0, 29], default: 23 (8MB)\n" + "  -fb{N}: set number of fast bytes - [5, 273], default: 128\n" + "  -lc{N}: set number of literal context bits - [0, 8], default: 3\n" + "  -lp{N}: set number of literal pos bits - [0, 4], default: 0\n" + "  -pb{N}: set number of pos bits - [0, 4], default: 2\n" + "  -mf{MF_ID}: set Match Finder: [bt2, bt4], default: bt4\n" + "  -eos:   write End Of Stream marker\n"

                // + " -si: read data from stdin\n"
                // + " -so: write data to stdout\n"
            );
        }

        private static int Start(string[] Args)
        {
            Console.WriteLine("\nLZMA# 4.61  2008-11-23\n");
            if (Args.Length == 0)
            {
                LzmaAlone.PrintHelp();
                return 0;
            }

            SwitchForm[] KSwitchForms = new SwitchForm[13];
            int sw = 0;
            KSwitchForms[sw++] = new SwitchForm("?", SwitchType.Simple, false);
            KSwitchForms[sw++] = new SwitchForm("H", SwitchType.Simple, false);
            KSwitchForms[sw++] = new SwitchForm("A", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("D", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("FB", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("LC", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("LP", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("PB", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("MF", SwitchType.UnLimitedPostString, false, 1);
            KSwitchForms[sw++] = new SwitchForm("EOS", SwitchType.Simple, false);
            KSwitchForms[sw++] = new SwitchForm("SI", SwitchType.Simple, false);
            KSwitchForms[sw++] = new SwitchForm("SO", SwitchType.Simple, false);
            KSwitchForms[sw++] = new SwitchForm("T", SwitchType.UnLimitedPostString, false, 1);
            Parser parser = new Parser(sw);
            try
            {
                parser.ParseStrings(KSwitchForms, Args);
            }
            catch
            {
                return LzmaAlone.IncorrectCommand();
            }

            if (parser[(int)Key.Help1].ThereIs || parser[(int)Key.Help2].ThereIs)
            {
                LzmaAlone.PrintHelp();
                return 0;
            }

            ArrayList NonSwitchStrings = parser.NonSwitchStrings;
            int ParamIndex = 0;
            if (ParamIndex >= NonSwitchStrings.Count)
            {
                return LzmaAlone.IncorrectCommand();
            }

            string command = (string)NonSwitchStrings[ParamIndex++];
            command = command.ToLower();
            bool DictionaryIsDefined = false;
            int dictionary = 1 << 21;
            if (parser[(int)Key.Dictionary].ThereIs)
            {
                int DicLog;
                if (!LzmaAlone.GetNumber((string)parser[(int)Key.Dictionary].PostStrings[0], out DicLog))
                {
                    LzmaAlone.IncorrectCommand();
                }

                dictionary = 1 << DicLog;
                DictionaryIsDefined = true;
            }

            string mf = "bt4";
            if (parser[(int)Key.MatchFinder].ThereIs)
            {
                mf = (string)parser[(int)Key.MatchFinder].PostStrings[0];
            }

            mf = mf.ToLower();
            if (command == "b")
            {
                const int KNumDefaultItereations = 10;
                int NumIterations = KNumDefaultItereations;
                if (ParamIndex < NonSwitchStrings.Count)
                {
                    if (!LzmaAlone.GetNumber((string)NonSwitchStrings[ParamIndex++], out NumIterations))
                    {
                        NumIterations = KNumDefaultItereations;
                    }
                }

                return LzmaBench.LzmaBenchmark(NumIterations, (UInt32)dictionary);
            }

            string train = string.Empty;
            if (parser[(int)Key.Train].ThereIs)
            {
                train = (string)parser[(int)Key.Train].PostStrings[0];
            }

            bool EncodeMode = false;
            if (command == "e")
            {
                EncodeMode = true;
            }
            else if (command == "d")
            {
                EncodeMode = false;
            }
            else
            {
                LzmaAlone.IncorrectCommand();
            }

            bool StdInMode = parser[(int)Key.StdIn].ThereIs;
            bool StdOutMode = parser[(int)Key.StdOut].ThereIs;
            Stream InStream = null;
            if (StdInMode)
            {
                throw new Exception("Not implemeted");
            }

            if (ParamIndex >= NonSwitchStrings.Count)
            {
                LzmaAlone.IncorrectCommand();
            }

            string InputName = (string)NonSwitchStrings[ParamIndex++];
            InStream = new FileStream(InputName, FileMode.Open, FileAccess.Read);
            FileStream OutStream = null;
            if (StdOutMode)
            {
                throw new Exception("Not implemeted");
            }

            if (ParamIndex >= NonSwitchStrings.Count)
            {
                LzmaAlone.IncorrectCommand();
            }

            string OutputName = (string)NonSwitchStrings[ParamIndex++];
            OutStream = new FileStream(OutputName, FileMode.Create, FileAccess.Write);
            FileStream TrainStream = null;
            if (train.Length != 0)
            {
                TrainStream = new FileStream(train, FileMode.Open, FileAccess.Read);
            }

            if (EncodeMode)
            {
                if (!DictionaryIsDefined)
                {
                    dictionary = 1 << 23;
                }

                int PosStateBits = 2;
                int LitContextBits = 3; // for normal files

                // UInt32 litContextBits = 0; // for 32-bit data
                int LitPosBits = 0;

                // UInt32 litPosBits = 2; // for 32-bit data
                int algorithm = 2;
                int NumFastBytes = 128;
                bool eos = parser[(int)Key.Eos].ThereIs || StdInMode;
                if (parser[(int)Key.Mode].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.Mode].PostStrings[0], out algorithm))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.FastBytes].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.FastBytes].PostStrings[0], out NumFastBytes))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.LitContext].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.LitContext].PostStrings[0], out LitContextBits))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.LitPos].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.LitPos].PostStrings[0], out LitPosBits))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.PosBits].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.PosBits].PostStrings[0], out PosStateBits))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                CoderPropId[] PropIDs =
                    {
                        CoderPropId.DictionarySize, CoderPropId.PosStateBits, CoderPropId.LitContextBits, CoderPropId.LitPosBits, CoderPropId.Algorithm, CoderPropId.NumFastBytes, CoderPropId.MatchFinder, CoderPropId.EndMarker
                    };
                object[] properties =
                    {
                        dictionary, PosStateBits, LitContextBits, LitPosBits, algorithm, NumFastBytes, mf, eos
                    };
                Encoder encoder = new Encoder();
                encoder.SetCoderProperties(PropIDs, properties);
                encoder.WriteCoderProperties(OutStream);
                long FileSize;
                if (eos || StdInMode)
                {
                    FileSize = -1;
                }
                else
                {
                    FileSize = InStream.Length;
                }

                for (int i = 0; i < 8; i++)
                {
                    OutStream.WriteByte((Byte)(FileSize >> (8 * i)));
                }

                if (TrainStream != null)
                {
                    CDoubleStream DoubleStream = new CDoubleStream();
                    DoubleStream.S1 = TrainStream;
                    DoubleStream.S2 = InStream;
                    DoubleStream.FileIndex = 0;
                    InStream = DoubleStream;
                    long TrainFileSize = TrainStream.Length;
                    DoubleStream.SkipSize = 0;
                    if (TrainFileSize > dictionary)
                    {
                        DoubleStream.SkipSize = TrainFileSize - dictionary;
                    }

                    TrainStream.Seek(DoubleStream.SkipSize, SeekOrigin.Begin);
                    encoder.SetTrainSize((uint)(TrainFileSize - DoubleStream.SkipSize));
                }

                encoder.Code(InStream, OutStream, -1, -1, null);
            }
            else if (command == "d")
            {
                byte[] properties = new byte[5];
                if (InStream.Read(properties, 0, 5) != 5)
                {
                    throw new Exception("input .lzma is too short");
                }

                Decoder decoder = new Decoder();
                decoder.SetDecoderProperties(properties);
                if (TrainStream != null)
                {
                    if (!decoder.Train(TrainStream))
                    {
                        throw new Exception("can't train");
                    }
                }

                long OutSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    int v = InStream.ReadByte();
                    if (v < 0)
                    {
                        throw new Exception("Can't Read 1");
                    }

                    OutSize |= (long)(byte)v << (8 * i);
                }

                long CompressedSize = InStream.Length - InStream.Position;
                decoder.Code(InStream, OutStream, CompressedSize, OutSize, null);
            }
            else
            {
                throw new Exception("Command Error");
            }

            return 0;
        }
    }
}