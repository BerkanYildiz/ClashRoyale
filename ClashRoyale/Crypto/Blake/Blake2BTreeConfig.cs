namespace ClashRoyale.Crypto.Blake
{
    public sealed class Blake2BTreeConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Blake2BTreeConfig"/> class.
        /// </summary>
        public Blake2BTreeConfig()
        {
            this.IntermediateHashSize = 64;
        }

        public int FanOut
        {
            get;
            set;
        }

        public int IntermediateHashSize
        {
            get;
            set;
        }

        public long LeafSize
        {
            get;
            set;
        }

        public int MaxHeight
        {
            get;
            set;
        }

        public static Blake2BTreeConfig CreateInterleaved(int Parallel)
        {
            Blake2BTreeConfig Result = new Blake2BTreeConfig
            {
                FanOut = Parallel, MaxHeight = 2, IntermediateHashSize = 64
            };
            return Result;
        }
    }
}