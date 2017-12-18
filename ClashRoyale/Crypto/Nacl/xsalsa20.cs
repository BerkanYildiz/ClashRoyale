namespace ClashRoyale.Crypto.Nacl
{
    public class Xsalsa20
    {
        public static readonly byte[] sigma =
        {
            (byte) 'e', (byte) 'x', (byte) 'p', (byte) 'a', (byte) 'n', (byte) 'd', (byte) ' ', (byte) '3', (byte) '2', (byte) '-', (byte) 'b', (byte) 'y', (byte) 't', (byte) 'e', (byte) ' ', (byte) 'k'
        };

        public readonly int CryptoStreamXsalsa20RefKeybytes = 32;
        public readonly int CryptoStreamXsalsa20RefNoncebytes = 24;

        public static int CryptoStream(byte[] C, int Clen, byte[] N, byte[] K)
        {
            byte[] subkey = new byte[32];

            Hsalsa20.CryptoCore(subkey, N, K, Xsalsa20.sigma);
            return Salsa20.CryptoStream(C, Clen, N, 16, subkey);
        }

        public static int CryptoStreamXor(byte[] C, byte[] M, long Mlen, byte[] N, byte[] K)
        {
            byte[] subkey = new byte[32];

            Hsalsa20.CryptoCore(subkey, N, K, Xsalsa20.sigma);
            return Salsa20.CryptoStreamXor(C, M, (int) Mlen, N, 16, subkey);
        }
    }
}