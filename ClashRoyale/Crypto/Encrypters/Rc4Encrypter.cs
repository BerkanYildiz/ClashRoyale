namespace ClashRoyale.Crypto.Encrypters
{
    public class Rc4Encrypter : IEncrypter
    {
        private byte I;
        private byte J;

        private byte[] Key;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rc4Encrypter"/> class.
        /// </summary>
        public Rc4Encrypter()
        {
            this.InitState("fhsd6f86f67rt8fw78fw789we78r9789wer6re", "nonce");
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Rc4Encrypter"/> class.
        /// </summary>
        /// <param name="Nonce">The nonce.</param>
        public Rc4Encrypter(string Nonce)
        {
            this.InitState("fhsd6f86f67rt8fw78fw789we78r9789wer6re", Nonce);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rc4Encrypter"/> class.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Nonce">The nonce.</param>
        public Rc4Encrypter(string Key, string Nonce)
        {
            this.InitState(Key, Nonce);
        }

        /// <summary>
        /// Initializes the rc4 encryption.
        /// </summary>
        private void InitState(string Key, string Nonce)
        {
            string Rc4Key = Key + Nonce;

            this.I = 0;
            this.J = 0;
            this.Key = new byte[256];

            for (int K = 0; K < 256; K++)
            {
                this.Key[K] = (byte) K;
            }

            byte J = 0;
            byte SwapTemp;

            for (int K = 0; K < 256; K++)
            {
                J = (byte) ((J + this.Key[K] + Rc4Key[K % Rc4Key.Length]) % 256);

                SwapTemp = this.Key[K];
                this.Key[K] = this.Key[J];
                this.Key[J] = SwapTemp;
            }

            for (int K = Rc4Key.Length; K > 0; K--)
            {
                this.I = (byte) (this.I + 1);
                this.J = (byte) (this.J + this.Key[this.I]);

                SwapTemp = this.Key[this.I];

                this.Key[this.I] = this.Key[this.J];
                this.Key[this.J] = SwapTemp;
            }
        }

        /// <summary>
        /// Decryptes the specified packet.
        /// </summary>
        public byte[] Decrypt(byte[] Packet)
        {
            if (Packet.Length > 0)
            {
                for (int K = 0; K < Packet.Length; K++)
                {
                    this.I = (byte) (this.I + 1);
                    this.J = (byte) (this.J + this.Key[this.I]);

                    byte SwapTemp = this.Key[this.I];

                    this.Key[this.I] = this.Key[this.J];
                    this.Key[this.J] = SwapTemp;

                    Packet[K] ^= this.Key[(this.Key[this.I] + this.Key[this.J]) % 256];
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
                for (int K = 0; K < Packet.Length; K++)
                {
                    this.I = (byte) (this.I + 1);
                    this.J = (byte) (this.J + this.Key[this.I]);

                    byte SwapTemp = this.Key[this.I];

                    this.Key[this.I] = this.Key[this.J];
                    this.Key[this.J] = SwapTemp;

                    Packet[K] ^= this.Key[(this.Key[this.I] + this.Key[this.J]) % 256];
                }
            }

            return Packet;
        }
    }
}