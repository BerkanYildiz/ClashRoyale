namespace ClashRoyale.Client.Packets.Crypto.Encrypter
{
    internal interface IEncrypter
    {
        bool IsRC4
        {
            get;
        }

        byte[] Decrypt(byte[] Packet);
        byte[] Encrypt(byte[] Packet);
    }
}