namespace ClashRoyale.Crypto.Encrypters
{
    using System;

    public class PepperEncrypter : IEncrypter
    {
        private readonly byte[] Nonce;
        private readonly byte[] SecretKey;

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

            for (int I = 0; I < 24; I++)
            {
                int Val = Add + this.Nonce[I];
                this.Nonce[I] = (byte) Val;
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

            for (int I = 0; I < 24; I++)
            {
                int Val = Add + this.Nonce[I];
                this.Nonce[I] = (byte) Val;
                Add = Val / 256;
            }

            return PepperCrypto.SecretBox(Packet, this.Nonce, this.SecretKey);
        }
    }
}