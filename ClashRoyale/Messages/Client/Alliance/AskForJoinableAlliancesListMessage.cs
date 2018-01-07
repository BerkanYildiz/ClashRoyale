namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForJoinableAlliancesListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10857;
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
        /// Initializes a new instance of the <see cref="AskForJoinableAlliancesListMessage"/> class.
        /// </summary>
        public AskForJoinableAlliancesListMessage()
        {
            // AskForJoinableAlliancesListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForJoinableAlliancesListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForJoinableAlliancesListMessage(ByteStream Stream) : base(Stream)
        {
            // AskForJoinableAlliancesListMessage.
        }
    }
}