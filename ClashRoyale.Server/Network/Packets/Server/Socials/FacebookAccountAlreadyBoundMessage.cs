namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class FacebookAccountAlreadyBoundMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24202;
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
