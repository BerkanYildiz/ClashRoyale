namespace ClashRoyale.Crypto.Inits
{
    public struct PepperInit
    {
        public byte State;
        public int KeyVersion;

        public byte[] Nonce;
        public byte[] SharedKey;
        public byte[] SessionKey;
        public byte[] ClientPublicKey;
        public byte[] ClientSecretKey;
        public byte[] ServerPublicKey;
    }
}