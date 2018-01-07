namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    public class EditAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14316;
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

        public string Description;

        public int HiringType;
        public int RequiredScore;

        public AllianceBadgeData Badge;
        public RegionData Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditAllianceMessage"/> class.
        /// </summary>
        public EditAllianceMessage()
        {
            // EditAllianceMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditAllianceMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public EditAllianceMessage(ByteStream Stream) : base(Stream)
        {
            // EditAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Description    = this.Stream.ReadString();
            this.Badge          = this.Stream.DecodeData<AllianceBadgeData>();
            this.HiringType     = this.Stream.ReadVInt();
            this.RequiredScore  = this.Stream.ReadVInt();
            this.Location       = this.Stream.DecodeData<RegionData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Description);
            this.Stream.EncodeData(this.Badge);
            this.Stream.WriteVInt(this.HiringType);
            this.Stream.WriteVInt(this.RequiredScore);
            this.Stream.EncodeData(this.Location);
        }
    }
}