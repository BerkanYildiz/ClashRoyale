namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;

    internal class AskForAvatarStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 17101;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarStreamMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        public AskForAvatarStreamMessage(Bot Bot) : base(Bot)
        {
            // AskForAvatarStreamMessage.
        }
    }
}