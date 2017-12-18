namespace ClashRoyale.Crypto.Blake
{
    using System.Security.Cryptography;

    public abstract class Blake2BBase
    {
        public HashAlgorithm AsHashAlgorithm()
        {
            return new HashAlgorithmAdapter(this);
        }

        public abstract byte[] Finish();

        /// <summary>
        /// Initialize the Blake2B Hasher.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Update the Blake2B Hasher configuration with the specified data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <param name="Start">The start.</param>
        /// <param name="Count">The count.</param>
        public abstract void Update(byte[] Data, int Start, int Count);

        /// <summary>
        /// Update the Blake2B Hasher using the specified data.
        /// </summary>
        /// <param name="Data">The data.</param>
        public void Update(byte[] Data)
        {
            this.Update(Data, 0, Data.Length);
        }

        public class HashAlgorithmAdapter : HashAlgorithm
        {
            private readonly Blake2BBase Hasher;

            public HashAlgorithmAdapter(Blake2BBase Hasher)
            {
                this.Hasher = Hasher;
            }

            public override void Initialize()
            {
                this.Hasher.Init();
            }

            protected override void HashCore(byte[] Array, int IbStart, int CbSize)
            {
                this.Hasher.Update(Array, IbStart, CbSize);
            }

            protected override byte[] HashFinal()
            {
                return this.Hasher.Finish();
            }
        }
    }
}