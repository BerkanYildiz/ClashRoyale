namespace ClashRoyale.Crypto.Nacl
{
    public class Hsalsa20
    {
        public const int ROUNDS = 20;

        public static int CryptoCore(byte[] Outv, byte[] Inv, byte[] K, byte[] C)
        {
            int x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15;
            int j0, j1, j2, j3, j4, j5, j6, j7, j8, j9, j10, j11, j12, j13, j14, j15;
            int i;

            j0 = x0 = Hsalsa20.LoadLittleendian(C, 0);
            j1 = x1 = Hsalsa20.LoadLittleendian(K, 0);
            j2 = x2 = Hsalsa20.LoadLittleendian(K, 4);
            j3 = x3 = Hsalsa20.LoadLittleendian(K, 8);
            j4 = x4 = Hsalsa20.LoadLittleendian(K, 12);
            j5 = x5 = Hsalsa20.LoadLittleendian(C, 4);

            if (Inv != null)
            {
                j6 = x6 = Hsalsa20.LoadLittleendian(Inv, 0);
                j7 = x7 = Hsalsa20.LoadLittleendian(Inv, 4);
                j8 = x8 = Hsalsa20.LoadLittleendian(Inv, 8);
                j9 = x9 = Hsalsa20.LoadLittleendian(Inv, 12);
            }
            else
            {
                j6 = x6 = j7 = x7 = j8 = x8 = j9 = x9 = 0;
            }

            j10 = x10 = Hsalsa20.LoadLittleendian(C, 8);
            j11 = x11 = Hsalsa20.LoadLittleendian(K, 16);
            j12 = x12 = Hsalsa20.LoadLittleendian(K, 20);
            j13 = x13 = Hsalsa20.LoadLittleendian(K, 24);
            j14 = x14 = Hsalsa20.LoadLittleendian(K, 28);
            j15 = x15 = Hsalsa20.LoadLittleendian(C, 12);

            for (i = Hsalsa20.ROUNDS; i > 0; i -= 2)
            {
                x4 ^= Hsalsa20.Rotate(x0 + x12, 7);
                x8 ^= Hsalsa20.Rotate(x4 + x0, 9);
                x12 ^= Hsalsa20.Rotate(x8 + x4, 13);
                x0 ^= Hsalsa20.Rotate(x12 + x8, 18);
                x9 ^= Hsalsa20.Rotate(x5 + x1, 7);
                x13 ^= Hsalsa20.Rotate(x9 + x5, 9);
                x1 ^= Hsalsa20.Rotate(x13 + x9, 13);
                x5 ^= Hsalsa20.Rotate(x1 + x13, 18);
                x14 ^= Hsalsa20.Rotate(x10 + x6, 7);
                x2 ^= Hsalsa20.Rotate(x14 + x10, 9);
                x6 ^= Hsalsa20.Rotate(x2 + x14, 13);
                x10 ^= Hsalsa20.Rotate(x6 + x2, 18);
                x3 ^= Hsalsa20.Rotate(x15 + x11, 7);
                x7 ^= Hsalsa20.Rotate(x3 + x15, 9);
                x11 ^= Hsalsa20.Rotate(x7 + x3, 13);
                x15 ^= Hsalsa20.Rotate(x11 + x7, 18);
                x1 ^= Hsalsa20.Rotate(x0 + x3, 7);
                x2 ^= Hsalsa20.Rotate(x1 + x0, 9);
                x3 ^= Hsalsa20.Rotate(x2 + x1, 13);
                x0 ^= Hsalsa20.Rotate(x3 + x2, 18);
                x6 ^= Hsalsa20.Rotate(x5 + x4, 7);
                x7 ^= Hsalsa20.Rotate(x6 + x5, 9);
                x4 ^= Hsalsa20.Rotate(x7 + x6, 13);
                x5 ^= Hsalsa20.Rotate(x4 + x7, 18);
                x11 ^= Hsalsa20.Rotate(x10 + x9, 7);
                x8 ^= Hsalsa20.Rotate(x11 + x10, 9);
                x9 ^= Hsalsa20.Rotate(x8 + x11, 13);
                x10 ^= Hsalsa20.Rotate(x9 + x8, 18);
                x12 ^= Hsalsa20.Rotate(x15 + x14, 7);
                x13 ^= Hsalsa20.Rotate(x12 + x15, 9);
                x14 ^= Hsalsa20.Rotate(x13 + x12, 13);
                x15 ^= Hsalsa20.Rotate(x14 + x13, 18);
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

            x0 -= Hsalsa20.LoadLittleendian(C, 0);
            x5 -= Hsalsa20.LoadLittleendian(C, 4);
            x10 -= Hsalsa20.LoadLittleendian(C, 8);
            x15 -= Hsalsa20.LoadLittleendian(C, 12);

            if (Inv != null)
            {
                x6 -= Hsalsa20.LoadLittleendian(Inv, 0);
                x7 -= Hsalsa20.LoadLittleendian(Inv, 4);
                x8 -= Hsalsa20.LoadLittleendian(Inv, 8);
                x9 -= Hsalsa20.LoadLittleendian(Inv, 12);
            }

            Hsalsa20.StoreLittleendian(Outv, 0, x0);
            Hsalsa20.StoreLittleendian(Outv, 4, x5);
            Hsalsa20.StoreLittleendian(Outv, 8, x10);
            Hsalsa20.StoreLittleendian(Outv, 12, x15);
            Hsalsa20.StoreLittleendian(Outv, 16, x6);
            Hsalsa20.StoreLittleendian(Outv, 20, x7);
            Hsalsa20.StoreLittleendian(Outv, 24, x8);
            Hsalsa20.StoreLittleendian(Outv, 28, x9);

            return 0;
        }

        public static int LoadLittleendian(byte[] X, int Offset)
        {
            return (X[Offset] & 0xff) | ((X[Offset + 1] & 0xff) << 8) | ((X[Offset + 2] & 0xff) << 16) | ((X[Offset + 3] & 0xff) << 24);
        }

        public static int Rotate(int U, int C)
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