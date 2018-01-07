namespace ClashRoyale.Messages.Server.Socials
{
    using ClashRoyale.Enums;

    public class FriendsInviteDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 22089;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        public string Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendsInviteDataMessage"/> class.
        /// </summary>
        /// <param name="Token">The token.</param>
        public FriendsInviteDataMessage(string Token)
        {
            this.Token = Token;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(true);
            this.Stream.WriteString(this.Token);
        }
    }
}