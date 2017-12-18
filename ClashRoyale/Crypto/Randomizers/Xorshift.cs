namespace ClashRoyale.Crypto.Randomizers
{
    using System;
    using System.Collections.Generic;

    public static class XorShift
    {
        private const double REAL_UNIT_INT = 1.0 / (int.MaxValue + 1.0);
        private const double REAL_UNIT_UINT = 1.0 / (uint.MaxValue + 1.0);

        private const uint Y = 842502087;
        private const uint Z = 3579807591;
        private const uint W = 273326509;

        private static uint _x;
        private static uint _y;
        private static uint _z;
        private static uint _w;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            XorShift.Reinitialise(Environment.TickCount);
        }

        /// <summary>
        /// Reinitialises the specified seed.
        /// </summary>
        /// <param name="Seed">The seed.</param>
        public static void Reinitialise(int Seed)
        {
            XorShift._x = (uint) Seed;

            XorShift._y = XorShift.Y;
            XorShift._z = XorShift.Z;
            XorShift._w = XorShift.W;
        }

        /// <summary>
        /// Generates a number between 0 and int.MaxValue.
        /// </summary>
        public static int Next()
        {
            while (true)
            {
                uint t = XorShift._x ^ XorShift._x << 11;
                XorShift._x = XorShift._y;
                XorShift._y = XorShift._z;
                XorShift._z = XorShift._w;
                XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8);

                uint rtn = XorShift._w & 0x7FFFFFFF;

                if (rtn != 0x7FFFFFFF)
                {
                    return (int) rtn;
                }
            }
        }

        public static int Next(int upperBound)
        {
            uint t = XorShift._x ^ XorShift._x << 11;
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;

            return (int)(XorShift.REAL_UNIT_INT * (int)(0x7FFFFFFF & (XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8))) * upperBound);
        }

        public static int Next(int lowerBound, int upperBound)
        {
            if (upperBound < lowerBound)
                return lowerBound;

            uint t = XorShift._x ^ XorShift._x << 11;
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;

            int range = upperBound - lowerBound;
            if (range < 0)
            {	
                return lowerBound + (int)(XorShift.REAL_UNIT_UINT * (XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8)) * ((long)upperBound - lowerBound));
            }

            return lowerBound + (int)(XorShift.REAL_UNIT_INT * (int)(0x7FFFFFFF & (XorShift._w = (XorShift._w ^ XorShift._w >> 19) ^ (t ^ t >> 8))) * range);
        }

        public static double NextDouble()
        {
            uint t = XorShift._x ^ XorShift._x << 11;
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;

            return XorShift.REAL_UNIT_INT * (int) (0x7FFFFFFF & (XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8)));
        }

        public static unsafe void NextBytes(byte[] buffer)
        {
            uint x = XorShift._x, y = XorShift._y, z = XorShift._z, w = XorShift._w;
            int i = 0;
            uint t;

            if (buffer.Length > 3)
            {
                fixed (byte* bptr = buffer)
                {
                    uint* iptr = (uint*)bptr;
                    uint* endptr = iptr + buffer.Length / 4;

                    do
                    {
                        t = (x ^ (x << 11));
                        x = y; y = z; z = w;
                        w = w ^ w >> 19 ^ (t ^ t >> 8);
                        *iptr = w;
                    }
                    while (++iptr < endptr);
                    i = buffer.Length - buffer.Length % 4;
                }
            }

            if (i < buffer.Length)
            {
                t = (x ^ (x << 11));
                x = y; y = z; z = w;
                w = w ^ w >> 19 ^ (t ^ t >> 8);
                do
                {
                    buffer[i] = (byte)(w >>= 8);
                } while (++i < buffer.Length);
            }
            XorShift._x = x; XorShift._y = y; XorShift._z = z; XorShift._w = w;
        }

        public static uint NextUInt()
        {
            uint t = XorShift._x ^ XorShift._x << 11;
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;

            return XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8);
        }

        public static int NextInt()
        {
            uint t = XorShift._x ^ XorShift._x << 11;
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;

            return (int) (0x7FFFFFFF & (XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8)));
        }

        static uint _bitBuffer;
        static uint _bitMask = 1;

        public static bool NextBool()
        {
            if (XorShift._bitMask != 1) return (XorShift._bitBuffer & (XorShift._bitMask >>= 1)) == 0;

            uint t = (XorShift._x ^ (XorShift._x << 11));
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;

            XorShift._bitBuffer = XorShift._w = (XorShift._w ^ (XorShift._w >> 19)) ^ (t ^ (t >> 8));

            XorShift._bitMask = 0x80000000;
            return (XorShift._bitBuffer & XorShift._bitMask) == 0;
        }

        static uint _byteBuffer;
        static uint _byteMove = 0;

        public static byte NextByte()
        {
            if (XorShift._byteMove != 0)
            {
                --XorShift._byteMove;
                return (byte) (XorShift._byteBuffer >>= 8);
            }
            uint t = XorShift._x ^ XorShift._x << 11;
            XorShift._x = XorShift._y;
            XorShift._y = XorShift._z;
            XorShift._z = XorShift._w;
            XorShift._byteBuffer = XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8);
            XorShift._byteMove = 3;
            return (byte) XorShift._byteBuffer;
        }

        public static T Next<T>(List<T> List)
        {
            return List[XorShift.Next(List.Count)];
        }

        public static string NextToken()
        {
            string Token = string.Empty;

            for (int i = 0; i < 40; i++)
            {
                uint t = XorShift._x ^ XorShift._x << 11;
                XorShift._x = XorShift._y;
                XorShift._y = XorShift._z;
                XorShift._z = XorShift._w;

                Token += (char) ('A' + (int) (XorShift.REAL_UNIT_UINT * (XorShift._w = XorShift._w ^ XorShift._w >> 19 ^ (t ^ t >> 8)) * ((long) 'Z' - 'A')));
            }

            return Token;
        }
    }
}