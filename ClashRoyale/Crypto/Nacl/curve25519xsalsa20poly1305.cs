namespace ClashRoyale.Crypto.Nacl
{
    using System;

    public class Curve25519Xsalsa20Poly1305
    {
        public const int crypto_secretbox_BEFORENMBYTES = 32;
        public const int crypto_secretbox_BOXZEROBYTES = 16;
        public const int crypto_secretbox_NONCEBYTES = 24;
        public const int crypto_secretbox_PUBLICKEYBYTES = 32;
        public const int crypto_secretbox_SECRETKEYBYTES = 32;
        public const int crypto_secretbox_ZEROBYTES = 32;

        public static int CryptoBox(byte[] C, byte[] M, long Mlen, byte[] N, byte[] Pk, byte[] Sk)
        {
            byte[] k = new byte[Curve25519Xsalsa20Poly1305.crypto_secretbox_BEFORENMBYTES];
            byte[] kp = k;

            Curve25519Xsalsa20Poly1305.CryptoBoxBeforenm(kp, Pk, Sk);
            return Curve25519Xsalsa20Poly1305.CryptoBoxAfternm(C, M, Mlen, N, kp);
        }

        public static int CryptoBox(byte[] C, byte[] M, byte[] N, byte[] Pk, byte[] Sk)
        {
            byte[] cp = C, mp = M, np = N, pkp = Pk, skp = Sk;
            return Curve25519Xsalsa20Poly1305.CryptoBox(cp, mp, M.Length, np, pkp, skp);
        }

        public static int CryptoBoxAfternm(byte[] C, byte[] M, long Mlen, byte[] N, byte[] K)
        {
            return Xsalsa20Poly1305.CryptoSecretbox(C, M, Mlen, N, K);
        }

        public static int CryptoBoxAfternm(byte[] C, byte[] M, byte[] N, byte[] K)
        {
            byte[] cp = C, mp = M, np = N, kp = K;
            return Curve25519Xsalsa20Poly1305.CryptoBoxAfternm(cp, mp, M.Length, np, kp);
        }

        public static int CryptoBoxBeforenm(byte[] K, byte[] Pk, byte[] Sk)
        {
            byte[] s = new byte[32];
            byte[] sp = s, sigmap = Xsalsa20.sigma;

            Curve25519.CryptoScalarmult(sp, Sk, Pk);
            return Hsalsa20.CryptoCore(K, null, sp, sigmap);
        }

        public static int CryptoBoxGetpublickey(byte[] Pk, byte[] Sk)
        {
            return Curve25519.CryptoScalarmultBase(Pk, Sk);
        }

        public static int CryptoBoxKeypair(byte[] Pk, byte[] Sk)
        {
            new Random().NextBytes(Sk);
            return Curve25519.CryptoScalarmultBase(Pk, Sk);
        }

        public static int CryptoBoxOpen(byte[] M, byte[] C, long Clen, byte[] N, byte[] Pk, byte[] Sk)
        {
            byte[] k = new byte[Curve25519Xsalsa20Poly1305.crypto_secretbox_BEFORENMBYTES];
            byte[] kp = k;

            Curve25519Xsalsa20Poly1305.CryptoBoxBeforenm(kp, Pk, Sk);
            return Curve25519Xsalsa20Poly1305.CryptoBoxOpenAfternm(M, C, Clen, N, kp);
        }

        public static int CryptoBoxOpen(byte[] M, byte[] C, byte[] N, byte[] Pk, byte[] Sk)
        {
            byte[] cp = C, mp = M, np = N, pkp = Pk, skp = Sk;
            return Curve25519Xsalsa20Poly1305.CryptoBoxOpen(mp, cp, C.Length, np, pkp, skp);
        }

        public static int CryptoBoxOpenAfternm(byte[] M, byte[] C, long Clen, byte[] N, byte[] K)
        {
            return Xsalsa20Poly1305.CryptoSecretboxOpen(M, C, Clen, N, K);
        }

        public static int CryptoBoxOpenAfternm(byte[] M, byte[] C, byte[] N, byte[] K)
        {
            byte[] cp = C, mp = M, np = N, kp = K;
            return Curve25519Xsalsa20Poly1305.CryptoBoxOpenAfternm(mp, cp, C.Length, np, kp);
        }
    }
}