namespace ClashRoyale.Maths
{
    using System.Runtime.InteropServices;

    using ClashRoyale.Extensions;

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public struct LogicLong
    {
        private int _HighInteger;
        private int _LowInteger;

        /// <summary>
        /// Gets the higher int of long.
        /// </summary>
        public int HigherInt
        {
            get
            {
                return this._HighInteger;
            }
        }

        /// <summary>
        /// Gets the lower int of long.
        /// </summary>
        public int LowerInt
        {
            get
            {
                return this._LowInteger;
            }
        }

        /// <summary>
        /// Gets if this instance is equal at 0.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this._HighInteger == 0 && this._LowInteger == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating the long value.
        /// </summary>
        public long Long
        {
            get
            {
                return (long) this._HighInteger << 32 | (uint) this._LowInteger;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLong"/> struct.
        /// </summary>
        /// <param name="Long">The long.</param>
        public LogicLong(long Long)
        {
            this._HighInteger   = (int) (Long >> 32);
            this._LowInteger    = (int) Long;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLong"/> struct.
        /// </summary>
        /// <param name="HighInteger">The high integer.</param>
        /// <param name="LowInteger">The low integer.</param>
        public LogicLong(int HighInteger, int LowInteger)
        {
            this._HighInteger   = HighInteger;
            this._LowInteger    = LowInteger;
        }

        public static LogicLong operator +(LogicLong Cmp, long Value)
        {
            return new LogicLong(Cmp.Long + Value);
        }

        public static LogicLong operator -(LogicLong Cmp, long Value)
        {
            return new LogicLong(Cmp.Long - Value);
        }

        public static LogicLong operator /(LogicLong Cmp, long Value)
        {
            return new LogicLong(Cmp.Long / Value);
        }

        public static LogicLong operator %(LogicLong Cmp, long Value)
        {
            return new LogicLong(Cmp.Long % Value);
        }

        public static LogicLong operator *(LogicLong Cmp, long Value)
        {
            return new LogicLong(Cmp.Long * Value);
        }

        public static implicit operator LogicLong(long Long)
        {
            return new LogicLong(Long);
        }

        public static implicit operator long(LogicLong Long)
        {
            return Long.Long;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public void Decode(ByteStream Stream)
        {
            this._HighInteger   = Stream.ReadVInt();
            this._LowInteger    = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this._HighInteger);
            Stream.WriteVInt(this._LowInteger);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return this._HighInteger + "-" + this._LowInteger;
        }

        /// <summary>
        /// Gets an empty instance of <see cref="LogicLong"/>.
        /// </summary>
        public static LogicLong Empty
        {
            get
            {
                return new LogicLong();
            }
        }
    }
}