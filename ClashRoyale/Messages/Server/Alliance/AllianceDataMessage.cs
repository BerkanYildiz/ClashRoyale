namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Alliance.Entries;

    public class AllianceDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 26550;
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

        public AllianceFullEntry AllianceFullEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataMessage"/> class.
        /// </summary>
        public AllianceDataMessage()
        {
            // AllianceDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AllianceDataMessage(ByteStream Stream) : base(Stream)
        {
            // AllianceDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataMessage"/> class.
        /// </summary>
        /// <param name="AllianceFullEntry">The alliance full entry.</param>
        public AllianceDataMessage(AllianceFullEntry AllianceFullEntry)
        {
            this.AllianceFullEntry = AllianceFullEntry;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.AllianceFullEntry.Decode(this.Stream);
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.AllianceFullEntry.Encode(this.Stream);
        }
    }
}