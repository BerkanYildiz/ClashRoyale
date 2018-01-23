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

        public string AllianceName;
        public string AllianceDescription;

        public int AllianceType;
        public int RequiredScore;

        public RegionData RegionData;
        public AllianceBadgeData AllianceBadgeData;

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
            this.AllianceName       = this.Stream.ReadString();
            this.AllianceDescription= this.Stream.ReadString();
            this.AllianceBadgeData  = this.Stream.DecodeData<AllianceBadgeData>();
            this.AllianceType       = this.Stream.ReadVInt();
            this.RequiredScore      = this.Stream.ReadVInt();
            this.RegionData         = this.Stream.DecodeData<RegionData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.AllianceName);
            this.Stream.WriteString(this.AllianceDescription);
            this.Stream.EncodeData(this.AllianceBadgeData);
            this.Stream.WriteVInt(this.AllianceType);
            this.Stream.WriteVInt(this.RequiredScore);
            this.Stream.EncodeData(this.RegionData);
        }
    }
}