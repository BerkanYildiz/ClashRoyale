namespace ClashRoyale.Server.Crypto.Nacl
{
    public class Verify16
    {
        internal readonly int CryptoVerify16RefBytes = 16;

        public static int CryptoVerify(byte[] X, int Xoffset, byte[] Y)
        {
            int differentbits = 0;

            for (int i = 0; i < 15; i++)
            {
                differentbits |= (X[Xoffset + i] ^ Y[i]) & 0xff;
            }

            return (1 & (int) ((uint) (differentbits - 1) >> 8)) - 1;
        }
    }
}