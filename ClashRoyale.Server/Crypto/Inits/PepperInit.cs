namespace ClashRoyale.Server.Crypto.Inits
{
    public struct PepperInit
    {
        internal byte State;
        internal int KeyVersion;

        internal byte[] Nonce;
        internal byte[] SharedKey;
        internal byte[] SessionKey;
        internal byte[] ClientPublicKey;
        internal byte[] ServerPublicKey;
    }
}