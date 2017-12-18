namespace ClashRoyale.Maths
{
    using ClashRoyale.Extensions;

    public class Random
    {
        public int Seed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        public Random()
        {
            // Random.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="Seed">The seed.</param>
        public Random(int Seed)
        {
            this.Seed = Seed;
        }

        /// <summary>
        /// Returns a random int beetween 0 and Max.
        /// </summary>
        public int Rand(int Max)
        {
            if (Max > 0)
            {
                if (this.Seed == 0)
                {
                    this.Seed = -1;
                }

                int Tmp = this.Seed ^ (this.Seed << 13) ^ ((this.Seed ^ (this.Seed << 13)) >> 17);
                this.Seed = Tmp ^ 32 * Tmp;

                if (this.Seed < 0)
                {
                    return -this.Seed % Max;
                }

                return this.Seed % Max;
            }

            return 0;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteInt(this.Seed);
        }
    }
}