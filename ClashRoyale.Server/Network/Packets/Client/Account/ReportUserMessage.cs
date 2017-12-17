namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class ReportUserMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10119;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportUserMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ReportUserMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // ReportUserMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Stream.ReadVInt(); // HighID
            this.Stream.ReadVInt(); // LowID
        }
    }
}