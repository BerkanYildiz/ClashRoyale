namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class DisconnectedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 25892;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public DisconnectedMessage(Device Device) : base(Device)
        {
            // DisconnectedMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(true);
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Stream.ReadBoolean();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            this.Device.State = State.Disconnected;
        }
    }
}