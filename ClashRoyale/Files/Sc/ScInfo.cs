namespace ClashRoyale.Files.Sc
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using ClashRoyale.Extensions;

    public class ScInfo : ScFile
    {
        public List<short> Identifiers;
        public List<string> Names;

        public int ReadedSheets;

        public List<Bitmap> Sheets;
        public List<ScSpirite> Spirites;
        public ScStats Statistics;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScInfo" /> class.
        /// </summary>
        public ScInfo()
        {
            this.Statistics = new ScStats();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScInfo" /> class.
        /// </summary>
        /// <param name="File">The file.</param>
        public ScInfo(FileInfo File) : base(File)
        {
            this.Statistics = new ScStats();

            this.Identifiers = new List<short>();
            this.Names = new List<string>();

            this.Sheets = new List<Bitmap>();
            this.Spirites = new List<ScSpirite>();
        }

        /// <summary>
        ///     Reads this instance.
        /// </summary>
        public override async Task Read()
        {
            using (BinaryReader Stream = new BinaryReader(this.File.OpenRead()))
            {
                byte[] Header = Stream.ReadBytes(2);

                if (Header[0] == (byte) 'S' || Header[1] == (byte) 'C')
                {
                    // Logging.Warning(this.GetType(), "The SC file is compressed, aborting.");
                    return;
                }

                Stream.BaseStream.Position = 0;

                this.Statistics.Decode(Stream);

                for (int i = 0; i < this.Statistics.StringsCount; i++)
                {
                    short Identifier = Stream.ReadInt16();

                    if (this.Identifiers.Find(T => T == Identifier) != 0)
                    {
                        Logging.Warning(this.GetType(), "Text #" + Identifier + " already in list.");
                    }

                    this.Identifiers.Add(Identifier);
                }

                for (int i = 0; i < this.Statistics.StringsCount; i++)
                {
                    string Name = Stream.ReadAscii();

                    if (string.IsNullOrEmpty(Name))
                    {
                        Logging.Warning(this.GetType(), "Text #" + i + " was either null or empty.");
                    }

                    this.Names.Add(Name);
                }

                while (Stream.BaseStream.Position < Stream.BaseStream.Length)
                {
                    byte BlockType = Stream.ReadByte();
                    int BlockLength = Stream.ReadInt32();
                    long BlockBegin = Stream.BaseStream.Position;

                    switch (BlockType)
                    {
                        case 0x01: // Sheets
                        {
                            byte Format = Stream.ReadByte();
                            short Width = Stream.ReadInt16();
                            short height = Stream.ReadInt16();

                            if (BlockLength > 5)
                            {
                                Logging.Warning(this.GetType(), "BlockLength > 5 at 0x01.");
                            }

                            ScTexture ScTexture = ScFiles.GetScTextureFile(this);

                            if (ScTexture != null)
                            {
                                Bitmap Texture = ScTexture.GetImage(this.ReadedSheets++);

                                if (Texture != null)
                                {
                                    this.Sheets.Add(Texture);
                                }
                                else
                                {
                                    Logging.Error(this.GetType(), "Texture == null at 0x01.");
                                }
                            }
                            else
                            {
                                Logging.Error(this.GetType(), "ScTexture == null at 0x01.");
                            }

                            break;
                        }

                        case 0x08:
                        {
                            float[] Points = new float[6];

                            for (int i = 0; i < 6; i++)
                            {
                                Points[i] = Stream.ReadInt32();
                            }

                            const double val1 = 0.00097656;
                            const double val2 = -20;

                            double[,] matrixArrayD =
                            {
                                {Points[0] * val1, Points[1] * val1, Points[2] * val1},
                                {Points[3] * val1, Points[4] / val2, Points[5] / val2}
                            };

                            //Matrix<double> Matrix = Matrix<double>.Build.DenseOfArray(matrixArrayD);

                            break;
                        }

                        case 0x09:
                        {
                            // Logging.Warning(this.GetType(), BitConverter.ToString(Stream.ReadBytes(BlockLength)));
                            break;
                        }

                        case 0x0C:
                        {
                            // Logging.Warning(this.GetType(), BitConverter.ToString(Stream.ReadBytes(BlockLength)));
                            break;
                        }

                        case 0x12: // Spirites
                        {
                            ScSpirite ScSpirite = new ScSpirite(this, BlockType);
                            ScSpirite.Decode(Stream);

                            this.Spirites.Add(ScSpirite);

                            break;
                        }

                        default:
                        {
                            Stream.ReadBytes(BlockLength);
                            break;
                        }
                    }

                    long BlockEnd = Stream.BaseStream.Position;
                    long BlockLeft = BlockEnd - BlockBegin;

                    if (BlockLeft != BlockLength)
                    {
                        if (BlockLeft > 0)
                        {
                            Logging.Error(this.GetType(), "We still have " + BlockLeft + " bytes to read !");
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "We are reading " + BlockLeft + " more bytes than we had to !");
                        }
                    }
                }
            }
        }
    }
}