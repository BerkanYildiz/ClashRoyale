namespace ClashRoyale.Extensions.Helper
{
    using System.Drawing;
    using System.IO;

    using ClashRoyale.Enums;

    public static class PixHelper
    {
        /// <summary>
        /// Gets the color for pixel format.
        /// </summary>
        /// <param name="PixelFormat">The tex reader.</param>
        /// <param name="Stream">The tex pixel format.</param>
        public static Color GetColor(BinaryReader Stream, int PixelFormat)
        {
            PixFormat Format = (PixFormat) PixelFormat;

            switch (Format)
            {
                case PixFormat.RGBA_8888:
                case PixFormat.RGBA_8888b:
                {
                    byte ColorR = Stream.ReadByte();
                    byte ColorG = Stream.ReadByte();
                    byte ColorB = Stream.ReadByte();
                    byte ColorA = Stream.ReadByte();

                    return Color.FromArgb((ColorA << 24) | (ColorR << 16) | (ColorG << 8) | ColorB);
                }

                case PixFormat.RGBA_4444:
                {
                    ushort RColor = Stream.ReadUInt16();

                    int ColorR = ((RColor >> 12) & 0xF) << 4;
                    int ColorG = ((RColor >> 8) & 0xF) << 4;
                    int ColorB = ((RColor >> 4) & 0xF) << 4;
                    int ColorA = (RColor & 0xF) << 4;

                    return Color.FromArgb(ColorA, ColorR, ColorG, ColorB);
                }

                case PixFormat.RGB_565:
                {
                    ushort _Color = Stream.ReadUInt16();

                    int Red     = ((_Color >> 11)   & 0x1F) << 3;
                    int Green   = ((_Color >> 5)    & 0x3F) << 2;
                    int Blue    = ((_Color & 0X1F) << 0x03);

                    return Color.FromArgb(Red, Green, Blue);
                }

                case PixFormat.LUMINANCE8_A8:
                {
                    ushort _Color = Stream.ReadUInt16();

                    int Alpha   = _Color & 0xFF;
                    int RGB     = _Color >> 8;

                    int Red     = RGB;
                    int Green   = RGB;
                    int Blue    = RGB;

                    return Color.FromArgb(Alpha, Red, Green, Blue);
                }

                case PixFormat.LUMINANCE8:
                {
                    ushort _Color = Stream.ReadByte();

                    int Red     = _Color;
                    int Green   = _Color;
                    int Blue    = _Color;

                    return Color.FromArgb(Red, Green, Blue);
                }

                default:
                {
                    Logging.Error(typeof(PixHelper), "Pixel Format n°" + PixelFormat + " is not supported.");
                }

                break;
            }

            return Color.Empty;
        }
    }
}
