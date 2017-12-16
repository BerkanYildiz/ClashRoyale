namespace ClashRoyale.Server.Logic
{
    using ClashRoyale.Server.Extensions;

    internal class DraftReward : Reward
    {
        private int ChestId;
        private int ChestGlobalId;
        private int ChestType;
        private int PaidGems;
        private int FreeGems;

        /// <summary>
        /// Gets the type of this reward.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DraftReward"/> class.
        /// </summary>
        public DraftReward()
        {
            // DraftReward.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.ChestId = Stream.ReadVInt();
            this.ChestGlobalId = Stream.ReadVInt();
            this.ChestType = Stream.ReadVInt();
            this.PaidGems = Stream.ReadVInt();
            this.FreeGems = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.ChestId);
            Stream.WriteVInt(this.ChestGlobalId);
            Stream.WriteVInt(this.ChestType);
            Stream.WriteVInt(this.PaidGems);
            Stream.WriteVInt(this.FreeGems);
        }
    }
}