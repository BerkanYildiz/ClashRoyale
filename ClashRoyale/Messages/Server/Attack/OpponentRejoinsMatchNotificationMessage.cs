namespace ClashRoyale.Messages.Server.Attack
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class OpponentRejoinsMatchNotificationMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20802;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpponentRejoinsMatchNotificationMessage"/> class.
        /// </summary>
        public OpponentRejoinsMatchNotificationMessage()
        {
            // OpponentRejoinsMatchNotificationMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpponentRejoinsMatchNotificationMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public OpponentRejoinsMatchNotificationMessage(ByteStream Stream) : base(Stream)
        {
            // OpponentRejoinsMatchNotificationMessage.
        }
    }
}