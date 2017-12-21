namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;

    internal class RequestApiKeyMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 15080;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestApiKeyMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        public RequestApiKeyMessage(Bot Bot) : base(Bot)
        {
            // RequestApiKeyMessage.
        }
    }
}