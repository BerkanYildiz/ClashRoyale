namespace ClashRoyale.Files.Sc
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;

    public class ScSpirite : ScObject
    {
        /// <summary>
        ///     Gets or sets the shape identifier.
        /// </summary>
        public short Identifier;

        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        public Bitmap Image;

        /// <summary>
        ///     Gets or sets the points count.
        /// </summary>
        public short PointsCount;

        /// <summary>
        ///     Gets or sets the UV points.
        /// </summary>
        public List<PointF> PointsUV;

        /// <summary>
        ///     Gets or sets the XY points.
        /// </summary>
        public List<PointF> PointsXY;

        /// <summary>
        ///     Gets or sets the polygon.
        /// </summary>
        public GraphicsPath Polygon;

        /// <summary>
        ///     Gets or sets the polygon count.
        /// </summary>
        public short PolygonCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScSpirite" /> class.
        /// </summary>
        public ScSpirite()
        {
            // ScSpirite.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScSpirite" /> class.
        /// </summary>
        /// <param name="BlockType">Type of the block.</param>
        public ScSpirite(short BlockType) : base(BlockType)
        {
            // ScSpirite.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScSpirite" /> class.
        /// </summary>
        /// <param name="ScFile">The sc file.</param>
        /// <param name="BlockType">Type of the block.</param>
        public ScSpirite(ScFile ScFile, short BlockType) : base(ScFile, BlockType)
        {
            // ScSpirite.
        }

        /// <summary>
        ///     Decodes this instance from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Decode(BinaryReader Stream)
        {
            this.Identifier = Stream.ReadInt16();
            this.PolygonCount = Stream.ReadInt16();

            if (this.Type == 0x12)
            {
                this.PointsCount = Stream.ReadInt16();
            }

            for (int i = 0; i < this.PolygonCount; i++)
            {
                this.DecodeSubBlock(Stream);
            }

            for (int i = 0; i < 5; i++)
            {
                Stream.ReadByte();
            }
        }

        /// <summary>
        ///     Encodes this instance in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(BinaryWriter Stream)
        {
            Stream.Write(this.Identifier);
            Stream.Write(this.PolygonCount);

            if (this.Type == 0x12)
            {
                Stream.Write(this.PointsCount);
            }

            for (int i = 0; i < this.PolygonCount; i++)
            {
                // Encode SubBlock.
            }
        }

        /// <summary>
        ///     Decodes the sub block using the specified reader.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal void DecodeSubBlock(BinaryReader Stream)
        {
            byte BlockType = Stream.ReadByte();
            int BlockLength = Stream.ReadInt32();

            long BlockStart = Stream.BaseStream.Position;

            // Logging.Info(this.GetType(), " -- SubBlock Type : 0x" + BlockType.ToString("X").PadLeft(2, '0') + ".");

            if (BlockLength > 0)
            {
                byte SheetId = Stream.ReadByte();
                byte Vertex = (byte) (BlockType == 0x04 ? 0x04 : Stream.ReadByte());

                ScTexture ScSheet = ScFiles.GetScTextureFile((ScInfo) this.ScFile);

                if (ScSheet == null)
                {
                    throw new Exception("ScSheet == null at DecodeSubBlock(BinaryReader Reader).");
                }

                Bitmap Sheet = ScSheet.GetImage(SheetId);

                if (Sheet == null)
                {
                    throw new Exception("Sheet == null at DecodeSubBlock(BinaryReader Reader).");
                }

                this.PointsXY = new List<PointF>(Vertex);

                for (int i = 0; i < Vertex; i++)
                {
                    float X = (float) Stream.ReadInt32() / -20;
                    float Y = (float) Stream.ReadInt32() / 20;

                    this.PointsXY.Add(new PointF(X, Y));
                }

                this.PointsUV = new List<PointF>(Vertex);

                for (int i = 0; i < Vertex; i++)
                {
                    float U = (float) Stream.ReadInt16() / ushort.MaxValue * Sheet.Width;
                    float V = (float) Stream.ReadInt16() / ushort.MaxValue * Sheet.Height;

                    this.PointsUV.Add(new PointF(U, V));
                }

                this.Polygon = new GraphicsPath();
                this.Polygon.AddPolygon(this.PointsUV.ToArray());

                RectangleF Bounds = this.Polygon.GetBounds();
                Rectangle BndRect = Rectangle.Round(Bounds);

                if (BndRect.IsEmpty == false)
                {
                    Bitmap ShrunkSheet = new Bitmap(BndRect.Width, BndRect.Height);

                    using (Graphics Graphic = Graphics.FromImage(ShrunkSheet))
                    {
                        // this.Polygon.Transform(new Matrix(1, 0, 0, 1, -BndRect.X, -BndRect.Y));

                        Graphic.SetClip(this.Polygon);
                        Graphic.DrawImage(Sheet, -BndRect.X, -BndRect.Y);
                    }

                    this.Image = ShrunkSheet;
                }
                else
                {
                    Logging.Warning(this.GetType(), "BndRect.IsEmpty == true at DecodeSubBlock(BinaryReader Reader).");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "BlockLength == 0 at DecodeSubBlock(BinaryReader Reader).");
            }

            long BlockEnd = Stream.BaseStream.Position;
            long BlockLeft = BlockEnd - BlockStart;

            if (BlockLeft == BlockLength)
            {
                // Logging.Info(this.GetType(), "BlockLeft == BlockLength at DecodeSubBlock(BinaryReader Reader).");
            }
            else
            {
                Logging.Error(this.GetType(), "BlockLeft != BlockLength at DecodeSubBlock(BinaryReader Reader).");
            }
        }
    }
}