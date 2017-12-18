namespace ClashRoyale.Crypto.Blake
{
    using System;

    public class Blake2BHasher : Blake2BBase
    {
        private readonly Blake2BConfig Config = new Blake2BConfig();
        private readonly Blake2BCore Core = new Blake2BCore();
        private readonly byte[] Key;
        private readonly int OutputSize;
        private readonly ulong[] RawConfig;

        /// <summary>
        /// Initialize a new instance of the <see cref="Blake2BHasher"/> class.
        /// </summary>
        public Blake2BHasher()
        {
            this.RawConfig = Blake2Builder.ConfigB(this.Config, null);

            if (this.Config.Key != null && this.Config.Key.Length != 0)
            {
                this.Key = new byte[24];
                Array.Copy(this.Config.Key, this.Key, this.Config.Key.Length);
            }

            this.OutputSize = this.Config.OutputSize;

            this.Init();
        }

        /// <summary>
        /// Finish this instance.
        /// </summary>
        /// <returns>The nonce.</returns>
        public override byte[] Finish()
        {
            byte[] FResult = this.Core.HashFinal();

            if (this.OutputSize != FResult.Length)
            {
                byte[] Result = new byte[this.OutputSize];
                Array.Copy(FResult, Result, Result.Length);
                return Result;
            }

            return FResult;
        }

        /// <summary>
        /// Initialize the Blake2B Hasher.
        /// </summary>
        public sealed override void Init()
        {
            this.Core.Initialize(this.RawConfig);

            if (this.Key != null)
            {
                this.Core.HashCore(this.Key, 0, this.Key.Length);
            }
        }

        /// <summary>
        /// Update the Blake2B Hasher with the specified data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <param name="Index">The index.</param>
        /// <param name="Count">The count.</param>
        public override void Update(byte[] Data, int Index, int Count)
        {
            this.Core.HashCore(Data, Index, Count);
        }
    }
}