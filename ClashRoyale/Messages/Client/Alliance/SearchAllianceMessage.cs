namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    public class SearchAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10949;
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

        public string Name;

        public int MinimumPlayers;
        public int MaximumPlayers;
        public int MinimumScore;

        public bool OpenOnly;

        public RegionData Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAllianceMessage"/> class.
        /// </summary>
        public SearchAllianceMessage()
        {
            // SearchAllianceMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAllianceMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public SearchAllianceMessage(ByteStream Stream) : base(Stream)
        {
            // SearchAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Name               = this.Stream.ReadString();
            this.Location           = this.Stream.DecodeData<RegionData>();

            this.MinimumPlayers     = this.Stream.ReadInt();
            this.MaximumPlayers     = this.Stream.ReadInt();
            this.MinimumScore       = this.Stream.ReadInt();

            this.OpenOnly           = this.Stream.ReadBoolean();

            this.Stream.ReadInt();
            this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Name);
            this.Stream.EncodeData(this.Location);

            this.Stream.WriteInt(this.MinimumPlayers);
            this.Stream.WriteInt(this.MaximumPlayers);
            this.Stream.WriteInt(this.MinimumScore);

            this.Stream.WriteBoolean(this.OpenOnly);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
        }
    }
}