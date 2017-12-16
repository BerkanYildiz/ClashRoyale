namespace ClashRoyale.Server.Network.Packets.Server.Socials
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class FacebookAccountBoundMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24201;
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
        /// Initializes a new instance of the <see cref="FacebookAccountBoundMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public FacebookAccountBoundMessage(Device Device) : base(Device)
        {
            // FacebookAccountBoundMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.ResultCode = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteInt(this.ResultCode);
        }
    }
}
