namespace ClashRoyale.Server.Network.Packets.Server.Socials
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class FriendsInviteDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20107;
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

        private readonly string Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendsInviteDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Token">The token.</param>
        public FriendsInviteDataMessage(Device Device, string Token) : base(Device)
        {
            this.Token      = Token;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteBoolean(true);
            this.Stream.WriteString(this.Token);
        }
    }
}