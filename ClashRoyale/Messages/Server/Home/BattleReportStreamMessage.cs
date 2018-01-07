namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class BattleReportStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20032;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        public long PlayerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleReportStreamMessage"/> class.
        /// </summary>
        public BattleReportStreamMessage()
        {
            // BattleReportStreamMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleReportStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public BattleReportStreamMessage(ByteStream Stream) : base(Stream)
        {
            // BattleReportStreamMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.PlayerId   = this.Stream.ReadLong();
            int EntryCount  = this.Stream.ReadVInt();

            for (int i = 0; i < EntryCount; i++)
            {
                // TODO : Read the entries.
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteLong(this.PlayerId);
            this.Stream.WriteVInt(0); // Count
        }
    }
}