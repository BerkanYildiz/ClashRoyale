namespace ClashRoyale.Server.Logic
{
    using System.Runtime.InteropServices;

    using ClashRoyale.Server.Extensions;

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    internal struct LogicLong
    {
        private int _HighInteger;
        private int _LowInteger;

        /// <summary>
        /// Gets the higher int of long.
        /// </summary>
        internal int HigherInt
        {
            get
            {
                return this._HighInteger;
            }
        }

        /// <summary>
        /// Gets the lower int of long.
        /// </summary>
        internal int LowerInt
        {
            get
            {
                return this._LowInteger;
            }
        }

        /// <summary>
        /// Gets if this instance is equal at 0.
        /// </summary>
        internal bool IsZero
        {
            get
            {
                return this._HighInteger == 0 && this._LowInteger == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating the long value.
        /// </summary>
        internal long Long
        {
            get
            {
                return (long)this._HighInteger << 32 | (uint)this._LowInteger;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LogicLong"/> structures.
        /// </summary>
        public LogicLong(long Long)
        {
            this._HighInteger = (int)(Long >> 32);
            this._LowInteger = (int)Long;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LogicLong"/> structures.
        /// </summary>
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
        internal void Decode(ByteStream Stream)
        {
            this._HighInteger   = Stream.ReadVInt();
            this._LowInteger    = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this._HighInteger);
            Stream.WriteVInt(this._LowInteger);
        }

        public override string ToString()
        {
            return this._HighInteger + "-" + this._LowInteger;
        }
    }
}