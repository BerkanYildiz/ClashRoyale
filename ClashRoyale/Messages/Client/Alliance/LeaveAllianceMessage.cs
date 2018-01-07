namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class LeaveAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14308;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveAllianceMessage"/> class.
        /// </summary>
        public LeaveAllianceMessage()
        {
            // LeaveAllianceMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveAllianceMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public LeaveAllianceMessage(ByteStream Stream) : base(Stream)
        {
            // LeaveAllianceMessage.
        }
    }
}