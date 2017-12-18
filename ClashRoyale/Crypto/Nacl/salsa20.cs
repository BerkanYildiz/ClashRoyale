namespace ClashRoyale.Crypto.Nacl
{
    public class Salsa20
    {
        public const int ROUNDS = 20;
        public readonly int CryptoCoreSalsa20RefConstbytes = 16;
        public readonly int CryptoCoreSalsa20RefInputbytes = 16;
        public readonly int CryptoCoreSalsa20RefKeybytes = 32;
        public readonly int CryptoCoreSalsa20RefOutputbytes = 64;
        public readonly int CryptoStreamSalsa20RefKeybytes = 32;
        public readonly int CryptoStreamSalsa20RefNoncebytes = 8;

        public static int CryptoCore(byte[] Outv, byte[] Inv, byte[] K, byte[] C)
        {
            int x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15;
            int j0, j1, j2, j3, j4, j5, j6, j7, j8, j9, j10, j11, j12, j13, j14, j15;
            int i;

            j0 = x0 = Salsa20.LoadLittleendian(C, 0);
            j1 = x1 = Salsa20.LoadLittleendian(K, 0);
            j2 = x2 = Salsa20.LoadLittleendian(K, 4);
            j3 = x3 = Salsa20.LoadLittleendian(K, 8);
            j4 = x4 = Salsa20.LoadLittleendian(K, 12);
            j5 = x5 = Salsa20.LoadLittleendian(C, 4);
            j6 = x6 = Salsa20.LoadLittleendian(Inv, 0);
            j7 = x7 = Salsa20.LoadLittleendian(Inv, 4);
            j8 = x8 = Salsa20.LoadLittleendian(Inv, 8);
            j9 = x9 = Salsa20.LoadLittleendian(Inv, 12);
            j10 = x10 = Salsa20.LoadLittleendian(C, 8);
            j11 = x11 = Salsa20.LoadLittleendian(K, 16);
            j12 = x12 = Salsa20.LoadLittleendian(K, 20);
            j13 = x13 = Salsa20.LoadLittleendian(K, 24);
            j14 = x14 = Salsa20.LoadLittleendian(K, 28);
            j15 = x15 = Salsa20.LoadLittleendian(C, 12);

            for (i = Salsa20.ROUNDS; i > 0; i -= 2)
            {
                x4 ^= (int) Salsa20.Rotate(x0 + x12, 7);
                x8 ^= (int) Salsa20.Rotate(x4 + x0, 9);
                x12 ^= (int) Salsa20.Rotate(x8 + x4, 13);
                x0 ^= (int) Salsa20.Rotate(x12 + x8, 18);
                x9 ^= (int) Salsa20.Rotate(x5 + x1, 7);
                x13 ^= (int) Salsa20.Rotate(x9 + x5, 9);
                x1 ^= (int) Salsa20.Rotate(x13 + x9, 13);
                x5 ^= (int) Salsa20.Rotate(x1 + x13, 18);
                x14 ^= (int) Salsa20.Rotate(x10 + x6, 7);
                x2 ^= (int) Salsa20.Rotate(x14 + x10, 9);
                x6 ^= (int) Salsa20.Rotate(x2 + x14, 13);
                x10 ^= (int) Salsa20.Rotate(x6 + x2, 18);
                x3 ^= (int) Salsa20.Rotate(x15 + x11, 7);
                x7 ^= (int) Salsa20.Rotate(x3 + x15, 9);
                x11 ^= (int) Salsa20.Rotate(x7 + x3, 13);
                x15 ^= (int) Salsa20.Rotate(x11 + x7, 18);
                x1 ^= (int) Salsa20.Rotate(x0 + x3, 7);
                x2 ^= (int) Salsa20.Rotate(x1 + x0, 9);
                x3 ^= (int) Salsa20.Rotate(x2 + x1, 13);
                x0 ^= (int) Salsa20.Rotate(x3 + x2, 18);
                x6 ^= (int) Salsa20.Rotate(x5 + x4, 7);
                x7 ^= (int) Salsa20.Rotate(x6 + x5, 9);
                x4 ^= (int) Salsa20.Rotate(x7 + x6, 13);
                x5 ^= (int) Salsa20.Rotate(x4 + x7, 18);
                x11 ^= (int) Salsa20.Rotate(x10 + x9, 7);
                x8 ^= (int) Salsa20.Rotate(x11 + x10, 9);
                x9 ^= (int) Salsa20.Rotate(x8 + x11, 13);
                x10 ^= (int) Salsa20.Rotate(x9 + x8, 18);
                x12 ^= (int) Salsa20.Rotate(x15 + x14, 7);
                x13 ^= (int) Salsa20.Rotate(x12 + x15, 9);
                x14 ^= (int) Salsa20.Rotate(x13 + x12, 13);
                x15 ^= (int) Salsa20.Rotate(x14 + x13, 18);
            }

            x0 += j0;
            x1 += j1;
            x2 += j2;
            x3 += j3;
            x4 += j4;
            x5 += j5;
            x6 += j6;
            x7 += j7;
            x8 += j8;
            x9 += j9;
            x10 += j10;
            x11 += j11;
            x12 += j12;
            x13 += j13;
            x14 += j14;
            x15 += j15;

            Salsa20.StoreLittleendian(Outv, 0, x0);
            Salsa20.StoreLittleendian(Outv, 4, x1);
            Salsa20.StoreLittleendian(Outv, 8, x2);
            Salsa20.StoreLittleendian(Outv, 12, x3);
            Salsa20.StoreLittleendian(Outv, 16, x4);
            Salsa20.StoreLittleendian(Outv, 20, x5);
            Salsa20.StoreLittleendian(Outv, 24, x6);
            Salsa20.StoreLittleendian(Outv, 28, x7);
            Salsa20.StoreLittleendian(Outv, 32, x8);
            Salsa20.StoreLittleendian(Outv, 36, x9);
            Salsa20.StoreLittleendian(Outv, 40, x10);
            Salsa20.StoreLittleendian(Outv, 44, x11);
            Salsa20.StoreLittleendian(Outv, 48, x12);
            Salsa20.StoreLittleendian(Outv, 52, x13);
            Salsa20.StoreLittleendian(Outv, 56, x14);
            Salsa20.StoreLittleendian(Outv, 60, x15);

            return 0;
        }

        public static int CryptoStream(byte[] C, int Clen, byte[] N, int Noffset, byte[] K)
        {
            byte[] inv = new byte[16];
            byte[] block = new byte[64];

            int coffset = 0;

            if (Clen == 0)
            {
                return 0;
            }

            for (int i = 0; i < 8; ++i)
            {
                inv[i] = N[Noffset + i];
            }

            for (int i = 8; i < 16; ++i)
            {
                inv[i] = 0;
            }

            while (Clen >= 64)
            {
                Salsa20.CryptoCore(C, inv, K, Xsalsa20.sigma);

                int u = 1;

                for (int i = 8; i < 16; ++i)
                {
                    u += inv[i] & 0xff;
                    inv[i] = (byte) u;
                    u = (int) ((uint) u >> 8);
                }

                Clen -= 64;
                coffset += 64;
            }

            if (Clen != 0)
            {
                Salsa20.CryptoCore(block, inv, K, Xsalsa20.sigma);

                for (int i = 0; i < Clen; ++i)
                {
                    C[coffset + i] = block[i];
                }
            }

            return 0;
        }

        public static int CryptoStreamXor(byte[] C, byte[] M, int Mlen, byte[] N, int Noffset, byte[] K)
        {
            byte[] inv = new byte[16];
            byte[] block = new byte[64];

            int coffset = 0;
            int moffset = 0;

            if (Mlen == 0)
            {
                return 0;
            }

            for (int i = 0; i < 8; ++i)
            {
                inv[i] = N[Noffset + i];
            }

            for (int i = 8; i < 16; ++i)
            {
                inv[i] = 0;
            }

            while (Mlen >= 64)
            {
                Salsa20.CryptoCore(block, inv, K, Xsalsa20.sigma);

                for (int i = 0; i < 64; ++i)
                {
                    C[coffset + i] = (byte) (M[moffset + i] ^ block[i]);
                }

                int u = 1;

                for (int i = 8; i < 16; ++i)
                {
                    u += inv[i] & 0xff;
                    inv[i] = (byte) u;
                    u = (int) ((uint) u >> 8);
                }

                Mlen -= 64;
                coffset += 64;
                moffset += 64;
            }

            if (Mlen != 0)
            {
                Salsa20.CryptoCore(block, inv, K, Xsalsa20.sigma);

                for (int i = 0; i < Mlen; ++i)
                {
                    C[coffset + i] = (byte) (M[moffset + i] ^ block[i]);
                }
            }

            return 0;
        }

        public static int LoadLittleendian(byte[] X, int Offset)
        {
            return (X[Offset] & 0xff) | ((X[Offset + 1] & 0xff) << 8) | ((X[Offset + 2] & 0xff) << 16) | ((X[Offset + 3] & 0xff) << 24);
        }

        public static long Rotate(int U, int C)
        {
            return (U << C) | (int) ((uint) U >> (32 - C));
        }

        public static void StoreLittleendian(byte[] X, int Offset, int U)
        {
            X[Offset] = (byte) U;
            U = (int) ((uint) U >> 8);
            X[Offset + 1] = (byte) U;
            U = (int) ((uint) U >> 8);
            X[Offset + 2] = (byte) U;
            U = (int) ((uint) U >> 8);
            X[Offset + 3] = (byte) U;
        }
    }
}