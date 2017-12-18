namespace ClashRoyale.Client.Packets.Crypto.Encrypter
{
    using System;

    internal class PepperEncrypter : IEncrypter
    {
        public bool IsRC4
        {
            get
            {
                return false;
            }
        }

        private byte[] Nonce;
        private byte[] SecretKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="PepperEncrypter"/> class.
        /// </summary>
        public PepperEncrypter(byte[] Nonce, byte[] SecretKey)
        {
            this.Nonce = new byte[24];
            this.SecretKey = new byte[32];

            Array.Copy(Nonce, this.Nonce, 24);
            Array.Copy(SecretKey, this.SecretKey, 32);
        }

        /// <summary>
        /// Decryptes the specified packet.
        /// </summary>
        public byte[] Decrypt(byte[] Packet)
        {
            int Add = 2;

            for (int i = 0; i < 24; i++)
            {
                int Val = Add + this.Nonce[i];
                this.Nonce[i] = (byte) Val;
                Add = Val / 256;
            }

            return PepperCrypto.SecretBoxOpen(Packet, this.Nonce, this.SecretKey);
        }

        /// <summary>
        /// Encryptes the specified packet.
        /// </summary>
        public byte[] Encrypt(byte[] Packet)
        {
            int Add = 2;

            for (int i = 0; i < 24; i++)
            {
                int Val = Add + this.Nonce[i];
                this.Nonce[i] = (byte) Val;
                Add = Val / 256;
            }
            
            return PepperCrypto.SecretBox(Packet, this.Nonce, this.SecretKey);
        }
    }
}