namespace ClashRoyale.Client.Packets.Crypto.Encrypter
{
    internal class RC4Encrypter : IEncrypter
    {
        public bool IsRC4
        {
            get
            {
                return true;
            }
        }

        private byte i;
        private byte j;

        private byte[] Key;

        /// <summary>
        /// Initializes a new instance of the <see cref="RC4Encrypter"/> class.
        /// </summary>
        public RC4Encrypter(string Key, string Nonce)
        {
            this.InitState(Key, Nonce);
        }

        /// <summary>
        /// Initializes the rc4 encryption.
        /// </summary>
        internal void InitState(string Key, string Nonce)
        {
            string RC4Key = Key + Nonce;

            this.i = 0;
            this.j = 0;
            this.Key = new byte[256];

            for (int k = 0; k < 256; k++)
            {
                this.Key[k] = (byte) k;
            }

            byte j = 0;
            byte swapTemp;

            for (int k = 0; k < 256; k++)
            {
                j = (byte) ((j + this.Key[k] + RC4Key[k % RC4Key.Length]) % 256);

                swapTemp = this.Key[k];
                this.Key[k] = this.Key[j];
                this.Key[j] = swapTemp;
            }

            for (int k = RC4Key.Length; k > 0; k--)
            {
                this.i = (byte) (this.i + 1);
                this.j = (byte) (this.j + this.Key[this.i]);

                byte SwapTemp = this.Key[this.i];

                this.Key[this.i] = this.Key[this.j];
                this.Key[this.j] = SwapTemp;
            }
        }

        /// <summary>
        /// Decryptes the specified packet.
        /// </summary>
        public byte[] Decrypt(byte[] Packet)
        {
            if (Packet.Length > 0)
            {
                for (int k = 0; k < Packet.Length; k++)
                {
                    this.i = (byte) (this.i + 1);
                    this.j = (byte) (this.j + this.Key[this.i]);

                    byte SwapTemp = this.Key[this.i];

                    this.Key[this.i] = this.Key[this.j];
                    this.Key[this.j] = SwapTemp;

                    Packet[k] ^= this.Key[(this.Key[this.i] + this.Key[this.j]) % 256];
                }
            }

            return Packet;
        }

        /// <summary>
        /// Encryptes the specified packet.
        /// </summary>
        public byte[] Encrypt(byte[] Packet)
        {
            if (Packet.Length > 0)
            {
                for (int k = 0; k < Packet.Length; k++)
                {
                    this.i = (byte)(this.i + 1);
                    this.j = (byte)(this.j + this.Key[this.i]);

                    byte SwapTemp = this.Key[this.i];

                    this.Key[this.i] = this.Key[this.j];
                    this.Key[this.j] = SwapTemp;

                    Packet[k] ^= this.Key[(this.Key[this.i] + this.Key[this.j]) % 256];
                }
            }

            return Packet;
        }
    }
}