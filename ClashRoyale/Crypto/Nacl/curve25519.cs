namespace ClashRoyale.Crypto.Nacl
{
    public class Curve25519
    {
        public static byte[] Basev =
        {
            9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        public static int[] Minusp =
        {
            19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128
        };

        public readonly int CryptoBytes = 32;
        public readonly int CryptoScalarbytes = 32;

        public static int CryptoScalarmult(byte[] Q, byte[] N, byte[] P)
        {
            int[] work = new int[96];
            byte[] e = new byte[32];

            for (int i = 0; i < 32; ++i)
            {
                e[i] = N[i];
            }

            e[0] &= 248;
            e[31] &= 127;
            e[31] |= 64;

            for (int i = 0; i < 32; ++i)
            {
                work[i] = P[i] & 0xFF;
            }

            Curve25519.Mainloop(work, e);

            Curve25519.Recip(work, 32, work, 32);
            Curve25519.Mult(work, 64, work, 0, work, 32);
            Curve25519.Freeze(work, 64);

            for (int i = 0; i < 32; ++i)
            {
                Q[i] = (byte) work[64 + i];
            }

            return 0;
        }

        public static int CryptoScalarmultBase(byte[] Q, byte[] N)
        {
            byte[] basevp = Curve25519.Basev;
            return Curve25519.CryptoScalarmult(Q, N, basevp);
        }

        public static void Add(int[] Outv, int Outvoffset, int[] A, int Aoffset, int[] B, int Boffset)
        {
            int u = 0;

            for (int j = 0; j < 31; ++j)
            {
                u += A[Aoffset + j] + B[Boffset + j];
                Outv[Outvoffset + j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += A[Aoffset + 31] + B[Boffset + 31];
            Outv[Outvoffset + 31] = u;
        }

        public static void Freeze(int[] A, int Aoffset)
        {
            int[] aorig = new int[32];

            for (int j = 0; j < 32; ++j)
            {
                aorig[j] = A[Aoffset + j];
            }

            int[] minuspp = Curve25519.Minusp;

            Curve25519.Add(A, 0, A, 0, minuspp, 0);

            int negative = -((int) ((uint) A[Aoffset + 31] >> 7) & 1);

            for (int j = 0; j < 32; ++j)
            {
                A[Aoffset + j] ^= negative & (aorig[j] ^ A[Aoffset + j]);
            }
        }

        public static void Mainloop(int[] Work, byte[] E)
        {
            int[] xzm1 = new int[64];
            int[] xzm = new int[64];
            int[] xzmb = new int[64];
            int[] Xzm1B = new int[64];
            int[] xznb = new int[64];
            int[] Xzn1B = new int[64];
            int[] a0 = new int[64];
            int[] a1 = new int[64];
            int[] b0 = new int[64];
            int[] b1 = new int[64];
            int[] c1 = new int[64];
            int[] r = new int[32];
            int[] s = new int[32];
            int[] t = new int[32];
            int[] u = new int[32];

            for (int j = 0; j < 32; ++j)
            {
                xzm1[j] = Work[j];
            }

            xzm1[32] = 1;

            for (int j = 33; j < 64; ++j)
            {
                xzm1[j] = 0;
            }

            xzm[0] = 1;

            for (int j = 1; j < 64; ++j)
            {
                xzm[j] = 0;
            }

            int[] xzmbp = xzmb, A0P = a0, Xzm1Bp = Xzm1B;
            int[] A1P = a1, B0P = b0, B1P = b1, C1P = c1;
            int[] xznbp = xznb, up = u, Xzn1Bp = Xzn1B;
            int[] workp = Work, sp = s, rp = r;

            for (int pos = 254; pos >= 0; --pos)
            {
                int b = (int) ((uint) (E[pos / 8] & 0xFF) >> (pos & 7));
                b &= 1;
                Curve25519.Select(xzmb, Xzm1B, xzm, xzm1, b);
                Curve25519.Add(a0, 0, xzmb, 0, xzmbp, 32);
                Curve25519.Sub(A0P, 32, xzmb, 0, xzmbp, 32);
                Curve25519.Add(a1, 0, Xzm1B, 0, Xzm1Bp, 32);
                Curve25519.Sub(A1P, 32, Xzm1B, 0, Xzm1Bp, 32);
                Curve25519.Square(B0P, 0, A0P, 0);
                Curve25519.Square(B0P, 32, A0P, 32);
                Curve25519.Mult(B1P, 0, A1P, 0, A0P, 32);
                Curve25519.Mult(B1P, 32, A1P, 32, A0P, 0);
                Curve25519.Add(c1, 0, b1, 0, B1P, 32);
                Curve25519.Sub(C1P, 32, b1, 0, B1P, 32);
                Curve25519.Square(rp, 0, C1P, 32);
                Curve25519.Sub(sp, 0, b0, 0, B0P, 32);
                Curve25519.Mult121665(t, s);
                Curve25519.Add(u, 0, t, 0, B0P, 0);
                Curve25519.Mult(xznbp, 0, B0P, 0, B0P, 32);
                Curve25519.Mult(xznbp, 32, sp, 0, up, 0);
                Curve25519.Square(Xzn1Bp, 0, C1P, 0);
                Curve25519.Mult(Xzn1Bp, 32, rp, 0, workp, 0);
                Curve25519.Select(xzm, xzm1, xznb, Xzn1B, b);
            }

            for (int j = 0; j < 64; ++j)
            {
                Work[j] = xzm[j];
            }
        }

        public static void Mult(int[] Outv, int Outvoffset, int[] A, int Aoffset, int[] B, int Boffset)
        {
            int j;

            for (int i = 0; i < 32; ++i)
            {
                int u = 0;

                for (j = 0; j <= i; ++j)
                {
                    u += A[Aoffset + j] * B[Boffset + i - j];
                }

                for (j = i + 1; j < 32; ++j)
                {
                    u += 38 * A[Aoffset + j] * B[Boffset + i + 32 - j];
                }

                Outv[Outvoffset + i] = u;
            }

            Curve25519.Squeeze(Outv, Outvoffset);
        }

        public static void Mult121665(int[] Outv, int[] A)
        {
            int j;
            int u = 0;

            for (j = 0; j < 31; ++j)
            {
                u += 121665 * A[j];
                Outv[j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += 121665 * A[31];
            Outv[31] = u & 127;
            u = 19 * (int) ((uint) u >> 7);

            for (j = 0; j < 31; ++j)
            {
                u += Outv[j];
                Outv[j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += Outv[j];
            Outv[j] = u;
        }

        public static void Recip(int[] Outv, int Outvoffset, int[] Z, int Zoffset)
        {
            int[] z2 = new int[32];
            int[] z9 = new int[32];
            int[] z11 = new int[32];
            int[] z2_5_0 = new int[32];
            int[] z2_10_0 = new int[32];
            int[] z2_20_0 = new int[32];
            int[] z2_50_0 = new int[32];
            int[] z2_100_0 = new int[32];
            int[] t0 = new int[32];
            int[] t1 = new int[32];

            /* 2 */
            int[] Z2P = z2;
            Curve25519.Square(Z2P, 0, Z, Zoffset);

            /* 4 */
            Curve25519.Square(t1, 0, z2, 0);

            /* 8 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 9 */
            int[] Z9P = z9, T0P = t0;
            Curve25519.Mult(Z9P, 0, T0P, 0, Z, Zoffset);

            /* 11 */
            Curve25519.Mult(z11, 0, z9, 0, z2, 0);

            /* 22 */
            Curve25519.Square(t0, 0, z11, 0);

            /* 2^5 - 2^0 = 31 */
            Curve25519.Mult(z2_5_0, 0, t0, 0, z9, 0);

            /* 2^6 - 2^1 */
            Curve25519.Square(t0, 0, z2_5_0, 0);

            /* 2^7 - 2^2 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^8 - 2^3 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^9 - 2^4 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^10 - 2^5 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^10 - 2^0 */
            Curve25519.Mult(z2_10_0, 0, t0, 0, z2_5_0, 0);

            /* 2^11 - 2^1 */
            Curve25519.Square(t0, 0, z2_10_0, 0);

            /* 2^12 - 2^2 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^20 - 2^10 */
            for (int i = 2; i < 10; i += 2)
            {
                Curve25519.Square(t0, 0, t1, 0);
                Curve25519.Square(t1, 0, t0, 0);
            }

            /* 2^20 - 2^0 */
            Curve25519.Mult(z2_20_0, 0, t1, 0, z2_10_0, 0);

            /* 2^21 - 2^1 */
            Curve25519.Square(t0, 0, z2_20_0, 0);

            /* 2^22 - 2^2 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^40 - 2^20 */
            for (int i = 2; i < 20; i += 2)
            {
                Curve25519.Square(t0, 0, t1, 0);
                Curve25519.Square(t1, 0, t0, 0);
            }

            /* 2^40 - 2^0 */
            Curve25519.Mult(t0, 0, t1, 0, z2_20_0, 0);

            /* 2^41 - 2^1 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^42 - 2^2 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^50 - 2^10 */
            for (int i = 2; i < 10; i += 2)
            {
                Curve25519.Square(t1, 0, t0, 0);
                Curve25519.Square(t0, 0, t1, 0);
            }

            /* 2^50 - 2^0 */
            Curve25519.Mult(z2_50_0, 0, t0, 0, z2_10_0, 0);

            /* 2^51 - 2^1 */
            Curve25519.Square(t0, 0, z2_50_0, 0);

            /* 2^52 - 2^2 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^100 - 2^50 */
            for (int i = 2; i < 50; i += 2)
            {
                Curve25519.Square(t0, 0, t1, 0);
                Curve25519.Square(t1, 0, t0, 0);
            }

            /* 2^100 - 2^0 */
            Curve25519.Mult(z2_100_0, 0, t1, 0, z2_50_0, 0);

            /* 2^101 - 2^1 */
            Curve25519.Square(t1, 0, z2_100_0, 0);

            /* 2^102 - 2^2 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^200 - 2^100 */
            for (int i = 2; i < 100; i += 2)
            {
                Curve25519.Square(t1, 0, t0, 0);
                Curve25519.Square(t0, 0, t1, 0);
            }

            /* 2^200 - 2^0 */
            Curve25519.Mult(t1, 0, t0, 0, z2_100_0, 0);

            /* 2^201 - 2^1 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^202 - 2^2 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^250 - 2^50 */
            for (int i = 2; i < 50; i += 2)
            {
                Curve25519.Square(t0, 0, t1, 0);
                Curve25519.Square(t1, 0, t0, 0);
            }

            /* 2^250 - 2^0 */
            Curve25519.Mult(t0, 0, t1, 0, z2_50_0, 0);

            /* 2^251 - 2^1 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^252 - 2^2 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^253 - 2^3 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^254 - 2^4 */
            Curve25519.Square(t0, 0, t1, 0);

            /* 2^255 - 2^5 */
            Curve25519.Square(t1, 0, t0, 0);

            /* 2^255 - 21 */
            int[] T1P = t1, Z11P = z11;
            Curve25519.Mult(Outv, Outvoffset, T1P, 0, Z11P, 0);
        }

        public static void Select(int[] P, int[] Q, int[] R, int[] S, int B)
        {
            int bminus1 = B - 1;

            for (int j = 0; j < 64; ++j)
            {
                int t = bminus1 & (R[j] ^ S[j]);
                P[j] = S[j] ^ t;
                Q[j] = R[j] ^ t;
            }
        }

        public static void Square(int[] Outv, int Outvoffset, int[] A, int Aoffset)
        {
            int j;

            for (int i = 0; i < 32; ++i)
            {
                int u = 0;

                for (j = 0; j < i - j; ++j)
                {
                    u += A[Aoffset + j] * A[Aoffset + i - j];
                }

                for (j = i + 1; j < i + 32 - j; ++j)
                {
                    u += 38 * A[Aoffset + j] * A[Aoffset + i + 32 - j];
                }

                u *= 2;

                if ((i & 1) == 0)
                {
                    u += A[Aoffset + i / 2] * A[Aoffset + i / 2];
                    u += 38 * A[Aoffset + i / 2 + 16] * A[Aoffset + i / 2 + 16];
                }

                Outv[Outvoffset + i] = u;
            }

            Curve25519.Squeeze(Outv, Outvoffset);
        }

        public static void Squeeze(int[] A, int Aoffset)
        {
            int u = 0;

            for (int j = 0; j < 31; ++j)
            {
                u += A[Aoffset + j];
                A[Aoffset + j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += A[Aoffset + 31];
            A[Aoffset + 31] = u & 127;
            u = 19 * (int) ((uint) u >> 7);

            for (int j = 0; j < 31; ++j)
            {
                u += A[Aoffset + j];
                A[Aoffset + j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += A[Aoffset + 31];
            A[Aoffset + 31] = u;
        }

        public static void Sub(int[] Outv, int Outvoffset, int[] A, int Aoffset, int[] B, int Boffset)
        {
            int u = 218;

            for (int j = 0; j < 31; ++j)
            {
                u += A[Aoffset + j] + 65280 - B[Boffset + j];
                Outv[Outvoffset + j] = u & 255;
                u = (int) ((uint) u >> 8);
            }

            u += A[Aoffset + 31] - B[Boffset + 31];
            Outv[Outvoffset + 31] = u;
        }
    }
}