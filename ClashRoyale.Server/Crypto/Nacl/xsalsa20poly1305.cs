namespace ClashRoyale.Server.Crypto.Nacl
{
    public class Xsalsa20Poly1305
    {
        internal readonly int CryptoSecretboxBoxzerobytes = 16;
        internal readonly int CryptoSecretboxKeybytes = 32;
        internal readonly int CryptoSecretboxNoncebytes = 24;
        internal readonly int CryptoSecretboxZerobytes = 32;

        public static int CryptoSecretbox(byte[] C, byte[] M, long Mlen, byte[] N, byte[] K)
        {
            if (Mlen < 32)
            {
                return -1;
            }

            Xsalsa20.CryptoStreamXor(C, M, Mlen, N, K);
            Poly1305.CryptoOnetimeauth(C, 16, C, 32, Mlen - 32, C);

            for (int i = 0; i < 16; ++i)
            {
                C[i] = 0;
            }

            return 0;
        }

        public static int CryptoSecretboxOpen(byte[] M, byte[] C, long Clen, byte[] N, byte[] K)
        {
            if (Clen < 32)
            {
                return -1;
            }

            byte[] subkeyp = new byte[32];

            Xsalsa20.CryptoStream(subkeyp, 32, N, K);

            if (Poly1305.CryptoOnetimeauthVerify(C, 16, C, 32, Clen - 32, subkeyp) != 0)
            {
                return -1;
            }

            Xsalsa20.CryptoStreamXor(M, C, Clen, N, K);

            for (int i = 0; i < 32; ++i)
            {
                M[i] = 0;
            }

            return 0;
        }
    }
}