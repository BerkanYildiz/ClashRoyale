namespace ClashRoyale.Compression.Lzma.Compress.LzmaAlone
{
    using System;
    using System.Collections;
    using System.IO;

    using ClashRoyale.Compression.Lzma.Common;
    using ClashRoyale.Compression.Lzma.Compress.LZMA;

    public class CDoubleStream : Stream
    {
        public int fileIndex;

        public Stream s1;

        public Stream s2;

        public long skipSize;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => this.s1.Length + this.s2.Length - this.skipSize;

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            int numTotal = 0;
            while (count > 0)
            {
                if (this.fileIndex == 0)
                {
                    int num = this.s1.Read(buffer, offset, count);
                    offset += num;
                    count -= num;
                    numTotal += num;
                    if (num == 0)
                    {
                        this.fileIndex++;
                    }
                }

                if (this.fileIndex == 1)
                {
                    numTotal += this.s2.Read(buffer, offset, count);
                    return numTotal;
                }
            }

            return numTotal;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("can't Seek");
        }

        public override void SetLength(long value)
        {
            throw new Exception("can't SetLength");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("can't Write");
        }
    }

    internal class LzmaAlone
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

            EOS,

            StdIn,

            StdOut,

            Train
        }

        private static bool GetNumber(string s, out int v)
        {
            v = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c < '0' || c > '9')
                {
                    return false;
                }

                v *= 10;
                v += c - '0';
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

        private static int Start(string[] args)
        {
            Console.WriteLine("\nLZMA# 4.61  2008-11-23\n");
            if (args.Length == 0)
            {
                LzmaAlone.PrintHelp();
                return 0;
            }

            SwitchForm[] kSwitchForms = new SwitchForm[13];
            int sw = 0;
            kSwitchForms[sw++] = new SwitchForm("?", SwitchType.Simple, false);
            kSwitchForms[sw++] = new SwitchForm("H", SwitchType.Simple, false);
            kSwitchForms[sw++] = new SwitchForm("A", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("D", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("FB", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("LC", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("LP", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("PB", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("MF", SwitchType.UnLimitedPostString, false, 1);
            kSwitchForms[sw++] = new SwitchForm("EOS", SwitchType.Simple, false);
            kSwitchForms[sw++] = new SwitchForm("SI", SwitchType.Simple, false);
            kSwitchForms[sw++] = new SwitchForm("SO", SwitchType.Simple, false);
            kSwitchForms[sw++] = new SwitchForm("T", SwitchType.UnLimitedPostString, false, 1);
            Parser parser = new Parser(sw);
            try
            {
                parser.ParseStrings(kSwitchForms, args);
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

            ArrayList nonSwitchStrings = parser.NonSwitchStrings;
            int paramIndex = 0;
            if (paramIndex >= nonSwitchStrings.Count)
            {
                return LzmaAlone.IncorrectCommand();
            }

            string command = (string)nonSwitchStrings[paramIndex++];
            command = command.ToLower();
            bool dictionaryIsDefined = false;
            int dictionary = 1 << 21;
            if (parser[(int)Key.Dictionary].ThereIs)
            {
                int dicLog;
                if (!LzmaAlone.GetNumber((string)parser[(int)Key.Dictionary].PostStrings[0], out dicLog))
                {
                    LzmaAlone.IncorrectCommand();
                }

                dictionary = 1 << dicLog;
                dictionaryIsDefined = true;
            }

            string mf = "bt4";
            if (parser[(int)Key.MatchFinder].ThereIs)
            {
                mf = (string)parser[(int)Key.MatchFinder].PostStrings[0];
            }

            mf = mf.ToLower();
            if (command == "b")
            {
                const int kNumDefaultItereations = 10;
                int numIterations = kNumDefaultItereations;
                if (paramIndex < nonSwitchStrings.Count)
                {
                    if (!LzmaAlone.GetNumber((string)nonSwitchStrings[paramIndex++], out numIterations))
                    {
                        numIterations = kNumDefaultItereations;
                    }
                }

                return LzmaBench.LzmaBenchmark(numIterations, (UInt32)dictionary);
            }

            string train = string.Empty;
            if (parser[(int)Key.Train].ThereIs)
            {
                train = (string)parser[(int)Key.Train].PostStrings[0];
            }

            bool encodeMode = false;
            if (command == "e")
            {
                encodeMode = true;
            }
            else if (command == "d")
            {
                encodeMode = false;
            }
            else
            {
                LzmaAlone.IncorrectCommand();
            }

            bool stdInMode = parser[(int)Key.StdIn].ThereIs;
            bool stdOutMode = parser[(int)Key.StdOut].ThereIs;
            Stream inStream = null;
            if (stdInMode)
            {
                throw new Exception("Not implemeted");
            }

            if (paramIndex >= nonSwitchStrings.Count)
            {
                LzmaAlone.IncorrectCommand();
            }

            string inputName = (string)nonSwitchStrings[paramIndex++];
            inStream = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            FileStream outStream = null;
            if (stdOutMode)
            {
                throw new Exception("Not implemeted");
            }

            if (paramIndex >= nonSwitchStrings.Count)
            {
                LzmaAlone.IncorrectCommand();
            }

            string outputName = (string)nonSwitchStrings[paramIndex++];
            outStream = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            FileStream trainStream = null;
            if (train.Length != 0)
            {
                trainStream = new FileStream(train, FileMode.Open, FileAccess.Read);
            }

            if (encodeMode)
            {
                if (!dictionaryIsDefined)
                {
                    dictionary = 1 << 23;
                }

                int posStateBits = 2;
                int litContextBits = 3; // for normal files

                // UInt32 litContextBits = 0; // for 32-bit data
                int litPosBits = 0;

                // UInt32 litPosBits = 2; // for 32-bit data
                int algorithm = 2;
                int numFastBytes = 128;
                bool eos = parser[(int)Key.EOS].ThereIs || stdInMode;
                if (parser[(int)Key.Mode].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.Mode].PostStrings[0], out algorithm))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.FastBytes].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.FastBytes].PostStrings[0], out numFastBytes))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.LitContext].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.LitContext].PostStrings[0], out litContextBits))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.LitPos].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.LitPos].PostStrings[0], out litPosBits))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                if (parser[(int)Key.PosBits].ThereIs)
                {
                    if (!LzmaAlone.GetNumber((string)parser[(int)Key.PosBits].PostStrings[0], out posStateBits))
                    {
                        LzmaAlone.IncorrectCommand();
                    }
                }

                CoderPropId[] propIDs =
                    {
                        CoderPropId.DictionarySize, CoderPropId.PosStateBits, CoderPropId.LitContextBits, CoderPropId.LitPosBits, CoderPropId.Algorithm, CoderPropId.NumFastBytes, CoderPropId.MatchFinder, CoderPropId.EndMarker
                    };
                object[] properties =
                    {
                        dictionary, posStateBits, litContextBits, litPosBits, algorithm, numFastBytes, mf, eos
                    };
                Encoder encoder = new Encoder();
                encoder.SetCoderProperties(propIDs, properties);
                encoder.WriteCoderProperties(outStream);
                long fileSize;
                if (eos || stdInMode)
                {
                    fileSize = -1;
                }
                else
                {
                    fileSize = inStream.Length;
                }

                for (int i = 0; i < 8; i++)
                {
                    outStream.WriteByte((Byte)(fileSize >> (8 * i)));
                }

                if (trainStream != null)
                {
                    CDoubleStream doubleStream = new CDoubleStream();
                    doubleStream.s1 = trainStream;
                    doubleStream.s2 = inStream;
                    doubleStream.fileIndex = 0;
                    inStream = doubleStream;
                    long trainFileSize = trainStream.Length;
                    doubleStream.skipSize = 0;
                    if (trainFileSize > dictionary)
                    {
                        doubleStream.skipSize = trainFileSize - dictionary;
                    }

                    trainStream.Seek(doubleStream.skipSize, SeekOrigin.Begin);
                    encoder.SetTrainSize((uint)(trainFileSize - doubleStream.skipSize));
                }

                encoder.Code(inStream, outStream, -1, -1, null);
            }
            else if (command == "d")
            {
                byte[] properties = new byte[5];
                if (inStream.Read(properties, 0, 5) != 5)
                {
                    throw new Exception("input .lzma is too short");
                }

                Decoder decoder = new Decoder();
                decoder.SetDecoderProperties(properties);
                if (trainStream != null)
                {
                    if (!decoder.Train(trainStream))
                    {
                        throw new Exception("can't train");
                    }
                }

                long outSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    int v = inStream.ReadByte();
                    if (v < 0)
                    {
                        throw new Exception("Can't Read 1");
                    }

                    outSize |= (long)(byte)v << (8 * i);
                }

                long compressedSize = inStream.Length - inStream.Position;
                decoder.Code(inStream, outStream, compressedSize, outSize, null);
            }
            else
            {
                throw new Exception("Command Error");
            }

            return 0;
        }
    }
}