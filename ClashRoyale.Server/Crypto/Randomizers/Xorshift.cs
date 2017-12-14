namespace ClashRoyale.Server.Crypto.Randomizers
{
    using System;
    using System.Collections.Generic;

    public class XorShift
    {
        private const double REAL_UNIT_INT = 1.0 / (int.MaxValue + 1.0);
        private const double REAL_UNIT_UINT = 1.0 / (uint.MaxValue + 1.0);

        private const uint Y = 842502087;
        private const uint Z = 3579807591;
        private const uint W = 273326509;

        private uint _x;
        private uint _y;
        private uint _z;
        private uint _w;

        /// <summary>
        /// Initializes a new instance of the <see cref="XorShift"/> class.
        /// </summary>
        public XorShift()
        {
            this.Reinitialise(Environment.TickCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XorShift"/> class.
        /// </summary>
        /// <param name="Seed">The seed.</param>
        public XorShift(int Seed)
        {
            this.Reinitialise(Seed);
        }

        /// <summary>
        /// Reinitialises the specified seed.
        /// </summary>
        /// <param name="Seed">The seed.</param>
        public void Reinitialise(int Seed)
        {
            this._x = (uint) Seed;

            this._y = XorShift.Y;
            this._z = XorShift.Z;
            this._w = XorShift.W;
        }

        /// <summary>
        /// Generates a number between 0 and int.MaxValue.
        /// </summary>
        public int Next()
        {
            while (true)
            {
                uint t = this._x ^ this._x << 11;
                this._x = this._y;
                this._y = this._z;
                this._z = this._w;
                this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8);

                uint rtn = this._w & 0x7FFFFFFF;

                if (rtn != 0x7FFFFFFF)
                {
                    return (int) rtn;
                }
            }
        }

        public int Next(int upperBound)
        {
            uint t = this._x ^ this._x << 11;
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;

            return (int)(XorShift.REAL_UNIT_INT * (int)(0x7FFFFFFF & (this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8))) * upperBound);
        }

        public int Next(int lowerBound, int upperBound)
        {
            if (upperBound < lowerBound)
                return lowerBound;

            uint t = this._x ^ this._x << 11;
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;

            int range = upperBound - lowerBound;
            if (range < 0)
            {	
                return lowerBound + (int)(XorShift.REAL_UNIT_UINT * (this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8)) * ((long)upperBound - lowerBound));
            }

            return lowerBound + (int)(XorShift.REAL_UNIT_INT * (int)(0x7FFFFFFF & (this._w = (this._w ^ this._w >> 19) ^ (t ^ t >> 8))) * range);
        }

        public double NextDouble()
        {
            uint t = this._x ^ this._x << 11;
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;

            return XorShift.REAL_UNIT_INT * (int) (0x7FFFFFFF & (this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8)));
        }

        public unsafe void NextBytes(byte[] buffer)
        {
            uint x = this._x, y = this._y, z = this._z, w = this._w;
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
            this._x = x; this._y = y; this._z = z; this._w = w;
        }

        public uint NextUInt()
        {
            uint t = this._x ^ this._x << 11;
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;

            return this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8);
        }

        public int NextInt()
        {
            uint t = this._x ^ this._x << 11;
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;

            return (int) (0x7FFFFFFF & (this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8)));
        }

        uint _bitBuffer;
        uint _bitMask = 1;

        public bool NextBool()
        {
            if (this._bitMask != 1) return (this._bitBuffer & (this._bitMask >>= 1)) == 0;

            uint t = (this._x ^ (this._x << 11));
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;

            this._bitBuffer = this._w = (this._w ^ (this._w >> 19)) ^ (t ^ (t >> 8));

            this._bitMask = 0x80000000;
            return (this._bitBuffer & this._bitMask) == 0;
        }

        uint _byteBuffer;
        uint _byteMove = 0;

        public byte NextByte()
        {
            if (this._byteMove != 0)
            {
                --this._byteMove;
                return (byte) (this._byteBuffer >>= 8);
            }
            uint t = this._x ^ this._x << 11;
            this._x = this._y;
            this._y = this._z;
            this._z = this._w;
            this._byteBuffer = this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8);
            this._byteMove = 3;
            return (byte) this._byteBuffer;
        }

        public T Next<T>(List<T> List)
        {
            return List[this.Next(List.Count)];
        }

        public string NextToken()
        {
            string Token = string.Empty;

            for (int i = 0; i < 40; i++)
            {
                uint t = this._x ^ this._x << 11;
                this._x = this._y;
                this._y = this._z;
                this._z = this._w;

                Token += (char) ('A' + (int) (XorShift.REAL_UNIT_UINT * (this._w = this._w ^ this._w >> 19 ^ (t ^ t >> 8)) * ((long) 'Z' - 'A')));
            }

            return Token;
        }
    }
}