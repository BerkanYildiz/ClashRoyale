namespace ClashRoyale.Server.Crypto.Encrypters
{
    internal interface IEncrypter
    {
        /// <summary>
        /// Decrypts the specified packet.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        byte[] Decrypt(byte[] Packet);

        /// <summary>
        /// Encrypts the specified packet.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        byte[] Encrypt(byte[] Packet);
    }
}