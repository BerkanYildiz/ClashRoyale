namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class GamecenterAccountBoundMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24211;
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
        /// Initializes a new instance of the <see cref="GamecenterAccountBoundMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public GamecenterAccountBoundMessage(Device Device) : base(Device)
        {
            // GamecenterAccountBoundMessage.
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
