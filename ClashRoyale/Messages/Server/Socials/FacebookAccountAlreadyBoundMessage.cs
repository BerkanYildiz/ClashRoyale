namespace ClashRoyale.Messages.Server.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;

    public class FacebookAccountAlreadyBoundMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24202;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        private int ResultCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAccountAlreadyBoundMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public FacebookAccountAlreadyBoundMessage(Device Device) : base(Device)
        {
            // FacebookAccountAlreadyBoundMessage.
        }
    }
}
