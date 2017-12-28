namespace ClashRoyale.Proxy.Packets
{
    using System.Linq;

    using ClashRoyale.Proxy.Network;

    internal class EnDecrypt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnDecrypt"/> class.
        /// </summary>
        internal EnDecrypt()
        {
            // EnDecrypt.
        }

        /// <summary>
        /// Decrypts the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        internal byte[] Decrypt(Packet Message)
        {
            return Message.Payload.ToArray();
        }

        /// <summary>
        /// Encrypts the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        internal byte[] Encrypt(Packet Message)
        {
            return Message.DecryptedData.ToArray();
        }
    }
}