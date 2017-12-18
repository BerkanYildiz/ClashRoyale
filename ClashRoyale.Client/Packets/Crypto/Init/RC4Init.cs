namespace ClashRoyale.Client.Packets.Crypto.Init
{
    internal struct RC4Init
    {
        internal int Seed;
        internal byte[] Nonce;

        internal bool Initialized;
    }
}