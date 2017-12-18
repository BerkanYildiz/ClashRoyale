namespace ClashRoyale.Crypto.Blake
{
    using System;

    public sealed partial class Blake2BCore
    {
        private const int BlockSizeInBytes = 128;

        private const ulong IV0 = 0x6A09E667F3BCC908UL;
        private const ulong IV1 = 0xBB67AE8584CAA73BUL;
        private const ulong IV2 = 0x3C6EF372FE94F82BUL;
        private const ulong IV3 = 0xA54FF53A5F1D36F1UL;
        private const ulong IV4 = 0x510E527FADE682D1UL;
        private const ulong IV5 = 0x9B05688C2B3E6C1FUL;
        private const ulong IV6 = 0x1F83D9ABFB41BD6BUL;
        private const ulong IV7 = 0x5BE0CD19137E2179UL;

        private bool IsInitialized;
        private int BufferFilled;
        private ulong Counter0;
        private ulong Counter1;
        private ulong FinalizationFlag0;
        private ulong FinalizationFlag1;

        private readonly byte[] Buf = new byte[128];
        private readonly ulong[] H = new ulong[8];
        private readonly ulong[] M = new ulong[16];

        public void HashCore(byte[] Array, int Start, int Count)
        {
            int offset = Start;
            int BufferRemaining = Blake2BCore.BlockSizeInBytes - this.BufferFilled;

            if (this.BufferFilled > 0 && Count > BufferRemaining)
            {
                System.Array.Copy(Array, offset, this.Buf, this.BufferFilled, BufferRemaining);

                this.Counter0 += Blake2BCore.BlockSizeInBytes;

                if (this.Counter0 == 0)
                {
                    this.Counter1++;
                }

                this.Compress(this.Buf, 0);

                offset += BufferRemaining;
                Count -= BufferRemaining;

                this.BufferFilled = 0;
            }

            while (Count > Blake2BCore.BlockSizeInBytes)
            {
                this.Counter0 += Blake2BCore.BlockSizeInBytes;

                if (this.Counter0 == 0)
                {
                    this.Counter1++;
                }

                this.Compress(Array, offset);

                offset += Blake2BCore.BlockSizeInBytes;
                Count -= Blake2BCore.BlockSizeInBytes;
            }

            if (Count > 0)
            {
                System.Array.Copy(Array, offset, this.Buf, this.BufferFilled, Count);
                this.BufferFilled += Count;
            }
        }

        public byte[] HashFinal()
        {
            return this.HashFinal(false);
        }

        public byte[] HashFinal(bool IsEndOfLayer)
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException("Not initialized");
            }

            this.IsInitialized = false;

            this.Counter0 += (uint) this.BufferFilled;
            this.FinalizationFlag0 = ulong.MaxValue;
            if (IsEndOfLayer)
            {
                this.FinalizationFlag1 = ulong.MaxValue;
            }

            for (int i = this.BufferFilled; i < this.Buf.Length; i++)
            {
                this.Buf[i] = 0;
            }

            this.Compress(this.Buf, 0);

            // Output
            byte[] hash = new byte[64];
            for (int i = 0; i < 8; ++i)
            {
                Blake2BCore.UInt64ToBytes(this.H[i], hash, i << 3);
            }

            return hash;
        }

        public void Initialize(ulong[] Config)
        {
            this.IsInitialized = true;

            this.H[0] = Blake2BCore.IV0;
            this.H[1] = Blake2BCore.IV1;
            this.H[2] = Blake2BCore.IV2;
            this.H[3] = Blake2BCore.IV3;
            this.H[4] = Blake2BCore.IV4;
            this.H[5] = Blake2BCore.IV5;
            this.H[6] = Blake2BCore.IV6;
            this.H[7] = Blake2BCore.IV7;

            this.Counter0 = 0;
            this.Counter1 = 0;
            this.FinalizationFlag0 = 0;
            this.FinalizationFlag1 = 0;

            this.BufferFilled = 0;

            Array.Clear(this.Buf, 0, this.Buf.Length);

            for (int i = 0; i < 8; i++)
            {
                this.H[i] ^= Config[i];
            }
        }

        public static ulong BytesToUInt64(byte[] Buf, int Offset)
        {
            return ((ulong) Buf[Offset + 7] << (7 * 8)) | ((ulong) Buf[Offset + 6] << (6 * 8)) | ((ulong) Buf[Offset + 5] << (5 * 8)) | ((ulong) Buf[Offset + 4] << (4 * 8)) | ((ulong) Buf[Offset + 3] << (3 * 8)) | ((ulong) Buf[Offset + 2] << (2 * 8)) | ((ulong) Buf[Offset + 1] << (1 * 8)) | Buf[Offset];
        }

        private static void UInt64ToBytes(ulong Value, byte[] Buf, int Offset)
        {
            Buf[Offset + 7] = (byte) (Value >> (7 * 8));
            Buf[Offset + 6] = (byte) (Value >> (6 * 8));
            Buf[Offset + 5] = (byte) (Value >> (5 * 8));
            Buf[Offset + 4] = (byte) (Value >> (4 * 8));
            Buf[Offset + 3] = (byte) (Value >> (3 * 8));
            Buf[Offset + 2] = (byte) (Value >> (2 * 8));
            Buf[Offset + 1] = (byte) (Value >> (1 * 8));
            Buf[Offset] = (byte) Value;
        }

        partial void Compress(byte[] Block, int Start);
    }
}