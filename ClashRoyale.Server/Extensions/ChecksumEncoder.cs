namespace ClashRoyale.Server.Extensions
{
    using System;

    internal class ChecksumEncoder
    {
        internal int Checksum;
        internal int BefChecksum;

        internal bool Enabled;

        internal ByteStream ByteStream;

        /// <summary>
        /// Gets if this instance is checksum only mode.
        /// </summary>
        internal virtual bool IsCheckSumOnlyMode
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumEncoder"/> class.
        /// </summary>
        /// <param name="ByteStream">The byte stream.</param>
        internal ChecksumEncoder(ByteStream ByteStream)
        {
            this.Enabled    = true;
            this.ByteStream = ByteStream;
        }

        /// <summary>
        /// Writes a byte value.
        /// </summary>
        internal virtual void WriteByte(byte Value)
        {
            this.Checksum = this.RotateRight(this.Checksum, 31) + Value + 11;
            this.ByteStream?.WriteByte(Value);
        }

        /// <summary>
        /// Writes a boolean value.
        /// </summary>
        internal virtual void WriteBoolean(bool Value)
        {
            this.Checksum = this.RotateRight(this.Checksum, 31) + (Value ? 13 : 7);
            this.ByteStream?.WriteBoolean(Value);
        }

        /// <summary>
        /// Writes a short value.
        /// </summary>
        internal virtual void WriteShort(short Value)
        {
            this.Checksum = this.RotateRight(this.Checksum, 31) + Value + 19;
            this.ByteStream?.WriteShort(Value);
        }

        /// <summary>
        /// Writes a int value.
        /// </summary>
        internal virtual void WriteInt(int Value)
        {
            this.Checksum = this.RotateRight(this.Checksum, 31) + Value + 9;
            this.ByteStream?.WriteInt(Value);
        }

        /// <summary>
        /// Writes a long value.
        /// </summary>
        internal virtual void WriteLong(long Value)
        {
            this.Checksum = (int) ((Value >> 32) + this.RotateRight((int) (Value >> 32) + this.RotateRight((int) Value, 31) + 67, 31) + 91);
            this.ByteStream?.WriteLong(Value);
        }

        /// <summary>
        /// Writes a byte array.
        /// </summary>
        internal virtual void WriteBytes(byte[] Buffer)
        {
            int Ror = this.RotateRight(this.Checksum, 31);

            if (Buffer != null)
            {
                this.Checksum = Ror + Buffer.Length + 28;
            }
            else
                this.Checksum = Ror + 27;

            this.ByteStream?.WriteBytes(Buffer);
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        internal virtual void WriteString(string String)
        {
            int Ror = this.RotateRight(this.Checksum, 31);

            if (String != null)
            {
                this.Checksum = Ror + String.Length + 28;
            }
            else
                this.Checksum = Ror + 27;

            this.ByteStream?.WriteString(String);
        }

        /// <summary>
        /// Writes a string reference.
        /// </summary>
        internal virtual void WriteStringReference(string String)
        {
            if (String == null)
            {
                throw new ArgumentNullException("String");
            }

            this.Checksum = this.RotateRight(this.Checksum, 31) + String.Length + 9;
            this.ByteStream?.WriteStringReference(String);
        }

        /// <summary>
        /// Writes a vint.
        /// </summary>
        internal virtual void WriteVInt(int Value)
        {
            this.Checksum = this.RotateRight(this.Checksum, 31) + Value + 33;
            this.ByteStream?.WriteVInt(Value);
        }

        /// <summary>
        /// Adds range to byte stream.
        /// </summary>
        internal virtual void AddRange(byte[] Packet)
        {
            this.ByteStream?.AddRange(Packet);
        }

        /// <summary>
        /// Sets if encoder is enabled.
        /// </summary>
        internal void EnableCheckSum(bool Value)
        {
            if (!this.Enabled || Value)
            {
                if (!this.Enabled && Value)
                {
                    this.Checksum = this.BefChecksum;
                }
            }
            else
                this.BefChecksum = this.Checksum;

            this.Enabled = Value;
        }

        /// <summary>
        /// Resets the checksum of this instance.
        /// </summary>
        internal void ResetChecksum()
        {
            this.Checksum = 0;
        }

        /// <summary>
        /// Sets the bytestream instance.
        /// </summary>
        internal void SetByteStream(ByteStream ByteStream)
        {
            this.ByteStream = ByteStream;
        }

        /// <summary>
        /// Rotates the integer.
        /// </summary>
        /// <param name="Value">The integer, aka checksum.</param>
        /// <param name="Count">The rotation count.</param>
        private int RotateRight(int Value, int Count)
        {
            return Value << Count | Value >> (32 - Count);
        }

        ~ChecksumEncoder()
        {
            this.ByteStream = null;
        }
    }
}