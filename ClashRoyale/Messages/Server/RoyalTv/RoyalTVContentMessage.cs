namespace ClashRoyale.Messages.Server.RoyalTv
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.RoyalTv;

    public class RoyalTvContentMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20073;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.RoyalTv;
            }
        }

        public RoyalTvEntry[] Entries;
        public ArenaData Arena;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoyalTvContentMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        /// <param name="Arena">The arena.</param>
        public RoyalTvContentMessage(RoyalTvEntry[] Entries, ArenaData Arena)
        {
            this.Arena   = Arena;
            this.Entries = Entries;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Entries = new RoyalTvEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.Entries.Length; i++)
            {
                RoyalTvEntry Entry = new RoyalTvEntry();
                Entry.Decode(this.Stream);
                this.Entries[i] = Entry;
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Entries.Length);

            foreach (RoyalTvEntry Entry in this.Entries)
            {
                Entry.Encode(this.Stream);
            }

            this.Stream.EncodeData(this.Arena);
        }
    }
}