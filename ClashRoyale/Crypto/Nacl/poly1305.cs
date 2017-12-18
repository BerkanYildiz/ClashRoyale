namespace ClashRoyale.Crypto.Nacl
{
    public class Poly1305
    {
        public static readonly int[] minusp =
        {
            5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 252
        };

        public readonly int CryptoBytes = 16;
        public readonly int CryptoKeybytes = 32;

        public static int CryptoOnetimeauth(byte[] Outv, int Outvoffset, byte[] Inv, int Invoffset, long Inlen, byte[] K)
        {
            int j;
            int[] r = new int[17];
            int[] h = new int[17];
            int[] c = new int[17];

            r[0] = K[0] & 0xFF;
            r[1] = K[1] & 0xFF;
            r[2] = K[2] & 0xFF;
            r[3] = K[3] & 15;
            r[4] = K[4] & 252;
            r[5] = K[5] & 0xFF;
            r[6] = K[6] & 0xFF;
            r[7] = K[7] & 15;
            r[8] = K[8] & 252;
            r[9] = K[9] & 0xFF;
            r[10] = K[10] & 0xFF;
            r[11] = K[11] & 15;
            r[12] = K[12] & 252;
            r[13] = K[13] & 0xFF;
            r[14] = K[14] & 0xFF;
            r[15] = K[15] & 15;
            r[16] = 0;

            for (j = 0; j < 17; ++j)
            {
                h[j] = 0;
            }

            while (Inlen > 0)
            {
                for (j = 0; j < 17; ++j)
                {
                    c[j] = 0;
                }

                for (j = 0; j < 16 && j < Inlen; ++j)
                {
                    c[j] = Inv[Invoffset + j] & 0xff;
                }

                c[j] = 1;
                Invoffset += j;
                Inlen -= j;
                Poly1305.Add(h, c);
                Poly1305.Mulmod(h, r);
            }

            Poly1305.Freeze(h);

            for (j = 0; j < 16; ++j)
            {
                c[j] = K[j + 16] & 0xFF;
            }

            c[16] = 0;
            Poly1305.Add(h, c);

            for (j = 0; j < 16; ++j)
            {
                Outv[j + Outvoffset] = (byte) h[j];
            }

            return 0;
        }

        public static int CryptoOnetimeauthVerify(byte[] H, int Hoffset, byte[] Inv, int Invoffset, long Inlen, byte[] K)
        {
            byte[] correct = new byte[16];

            Poly1305.CryptoOnetimeauth(correct, 0, Inv, Invoffset, Inlen, K);
            return Verify16.CryptoVerify(H, Hoffset, correct);
        }

        public static void Add(int[] H, int[] C)
        {
            int j;
            int u = 0;

            for (j = 0; j < 17; ++j)
            {
                u += H[j] + C[j];
                H[j] = u & 255;
                u = (int) ((uint) u >> 8);
            }
        }

        public static void Freeze(int[] H)
        {
            int[] horig = new int[17];

            for (int j = 0; j < 17; ++j)
            {
                horig[j] = H[j];
            }

            Poly1305.Add(H, Poly1305.minusp);

            int negative = -(int) ((uint) H[16] >> 7);

            for (int j = 0; j < 17; ++j)
            {
                H[j] ^= negative & (horig[j] ^ H[j]);
            }
        }

        public static void Mulmod(int[] H, int[] R)
        {
            int[] hr = new int[17];

            for (int i = 0; i < 17; ++i)
            {
                int u = 0;

                for (int j = 0; j <= i; ++j)
                {
                    u += H[j] * R[i - j];
                }

                for (int j = i + 1; j < 17; ++j)
                {
                    u += 320 * H[j] * R[i + 17 - j];
                }

                hr[i] = u;
            }

            for (int i = 0; i < 17; ++i)
            {
                H[i] = hr[i];
            }

            Poly1305.Squeeze(H);
        }

        public static void Squeeze(int[] H)
        {
            int u = 0;

            for (int j = 0; j < 16; ++j)
            {
                u += H[j];
                H[j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += H[16];
            H[16] = u & 3;
            u = 5 * (int) ((uint) u >> 2);

            for (int j = 0; j < 16; ++j)
            {
                u += H[j];
                H[j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += H[16];
            H[16] = u;
        }
    }
}