namespace ClashRoyale.Files.Sc
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ClashRoyale.Extensions.Helper;

    public class ScTexture : ScFile
    {
        public readonly List<Bitmap> Sheets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScTexture" /> class.
        /// </summary>
        public ScTexture()
        {
            this.Sheets = new List<Bitmap>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScTexture" /> class.
        /// </summary>
        /// <param name="File">The file.</param>
        public ScTexture(FileInfo File) : base(File)
        {
            this.Sheets = new List<Bitmap>();
        }

        /// <summary>
        ///     Gets the <see cref="Bitmap" /> at the specified offset.
        /// </summary>
        /// <value>
        ///     The <see cref="Bitmap" />.
        /// </value>
        /// <param name="Offset">The offset.</param>
        public Bitmap this[int Offset]
        {
            get
            {
                return this.Sheets[Offset];
            }
        }

        /// <summary>
        ///     Reads this instance.
        /// </summary>
        public virtual async Task Read()
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

                while (Stream.BaseStream.Position < Stream.BaseStream.Length)
                {
                    byte PacketId = Stream.ReadByte();
                    uint PacketSize = Stream.ReadUInt16();

                    Logging.Info(this.GetType(), "ID : " + PacketId + ", SIZE : " + PacketSize + ".");

                    if (PacketSize > 0)
                    {
                        byte PixFormat = Stream.ReadByte();

                        ushort Width = Stream.ReadUInt16();
                        ushort Height = Stream.ReadUInt16();

                        bool Is32x32 = false;

                        switch (PacketId)
                        {
                            case 27:
                            case 28:
                            {
                                Is32x32 = true;
                                break;
                            }
                        }

                        Bitmap Sheet = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

                        int ModWidth = Width % 32;
                        int TimeWidth = (Width - ModWidth) / 32;

                        int ModHeight = Height % 32;
                        int TimeHeight = (Height - ModHeight) / 32;

                        Color[,] Pixels = new Color[Height, Width];

                        if (Is32x32)
                        {
                            for (int TimeH = 0; TimeH < TimeHeight + 1; TimeH++)
                            {
                                int OffsetX;
                                int OffsetY;

                                int LineH = 32;

                                if (TimeH == TimeHeight)
                                {
                                    LineH = ModHeight;
                                }

                                for (int Time = 0; Time < TimeWidth; Time++)
                                {
                                    for (int PositionY = 0; PositionY < LineH; PositionY++)
                                    {
                                        for (int PositionX = 0; PositionX < 32; PositionX++)
                                        {
                                            OffsetX = Time * 32;
                                            OffsetY = TimeH * 32;

                                            Pixels[PositionY + OffsetY, PositionX + OffsetX] = PixHelper.GetColor(Stream, PixFormat);
                                        }
                                    }
                                }

                                for (int PositionY = 0; PositionY < LineH; PositionY++)
                                {
                                    for (int PositionX = 0; PositionX < ModWidth; PositionX++)
                                    {
                                        OffsetX = TimeWidth * 32;
                                        OffsetY = TimeH * 32;

                                        Pixels[PositionY + OffsetY, PositionX + OffsetX] = PixHelper.GetColor(Stream, PixFormat);
                                    }
                                }
                            }
                        }

                        for (int Row = 0; Row < Pixels.GetLength(0); Row++)
                        {
                            for (int Column = 0; Column < Pixels.GetLength(1); Column++)
                            {
                                Sheet.SetPixel(Column, Row, Is32x32 ? Pixels[Row, Column] : PixHelper.GetColor(Stream, PixFormat));
                            }
                        }

                        this.Sheets.Add(Sheet);
                    }
                    else
                    {
                        bool IsValid = true;

                        byte[] Checksum = Stream.ReadBytes(5);

                        foreach (byte Digit in Checksum)
                        {
                            if (Digit != 0x00)
                            {
                                IsValid = false;
                            }
                        }

                        if (IsValid != true)
                        {
                            Logging.Error(this.GetType(), "Checksum is not valid.");
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the texture image for the given texture identifier.
        /// </summary>
        /// <param name="TextureId">The texture identifier.</param>
        /// <exception cref="System.Exception">Expected a non-negative value.</exception>
        public Bitmap GetImage(int TextureId)
        {
            if (TextureId < 0)
            {
                throw new Exception("Expected a non-negative value.");
            }

            if (TextureId < this.Sheets.Count)
            {
                return this.Sheets[TextureId];
            }

            return null;
        }

        /// <summary>
        ///     Gets the texture image for the given texture identifier.
        /// </summary>
        /// <param name="TextureId">The texture identifier.</param>
        /// <exception cref="System.Exception">Expected a non-negative value.</exception>
        public bool TryGetImage(int TextureId, out Bitmap Sheet)
        {
            if (TextureId < 0)
            {
                throw new Exception("Expected a non-negative value.");
            }

            Sheet = null;

            if (this.Sheets.Count <= TextureId)
            {
                Sheet = this.Sheets[TextureId];
            }

            return Sheet != null;
        }

        /// <summary>
        ///     Gets the textures images for this <see cref="ScTexture" /> file.
        /// </summary>
        public List<Bitmap> GetImages()
        {
            if (this.Sheets.Count > 0)
            {
                return this.Sheets.ToList();
            }

            return null;
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            DirectoryInfo Dir = Directory.CreateDirectory("Textures/" + this.ScName + "_tex" + (this.IsMultiRes ? (this.IsHighRes ? "_highres_" : "_lowres_") + ".sc" : ".sc"));

            foreach (Bitmap Sheet in this.Sheets)
            {
                Sheet.Save(Dir.FullName + "/texture_" + this.Sheets.IndexOf(Sheet) + ".png", ImageFormat.Png);
            }
        }
    }
}