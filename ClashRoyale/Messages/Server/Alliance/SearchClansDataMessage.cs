namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Alliance.Entries;

    public class SearchClansDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24310;
            }
        }

        /// <summary> 
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        public AllianceHeaderEntry[] Entries;
        public string Filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchClansDataMessage"/> class.
        /// </summary>
        public SearchClansDataMessage()
        {
            // SearchClansDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchClansDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public SearchClansDataMessage(ByteStream Stream) : base(Stream)
        {
            // SearchClansDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchClansDataMessage"/> class.
        /// </summary>
        /// <param name="Filter">The filter.</param>
        /// <param name="Entries">The entries.</param>
        public SearchClansDataMessage(string Filter, AllianceHeaderEntry[] Entries)
        {
            this.Filter  = Filter;
            this.Entries = Entries;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Filter);
            this.Stream.WriteVInt(this.Entries.Length);

            foreach (var Entry in this.Entries)
            {
                Entry.Encode(this.Stream);
            }
        }
    }
}