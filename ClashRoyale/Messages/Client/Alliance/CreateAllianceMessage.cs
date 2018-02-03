namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    public class CreateAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 11033;
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
        public string Description;

        public int AccessType;
        public int RequiredScore;

        public RegionData RegionData;
        public AllianceBadgeData BadgeData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAllianceMessage"/> class.
        /// </summary>
        public CreateAllianceMessage()
        {
            // CreateAllianceMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAllianceMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public CreateAllianceMessage(ByteStream Stream) : base(Stream)
        {
            // CreateAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Name           = this.Stream.ReadString();
            this.Description    = this.Stream.ReadString();
            this.BadgeData      = this.Stream.DecodeData<AllianceBadgeData>();
            this.AccessType     = this.Stream.ReadVInt();
            this.RequiredScore  = this.Stream.ReadVInt();
            this.RegionData     = this.Stream.DecodeData<RegionData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Name);
            this.Stream.WriteString(this.Description);
            this.Stream.EncodeData(this.BadgeData);
            this.Stream.WriteVInt(this.AccessType);
            this.Stream.WriteVInt(this.RequiredScore);
            this.Stream.EncodeData(this.RegionData);
        }
    }
}