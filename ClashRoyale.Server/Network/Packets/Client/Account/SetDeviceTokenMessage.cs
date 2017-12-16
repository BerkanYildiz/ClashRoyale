namespace ClashRoyale.Server.Network.Packets.Client.Account
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class SetDeviceTokenMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10113;
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

        private string ServerToken;
        private string ClientToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetDeviceTokenMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public SetDeviceTokenMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // SetDeviceTokenMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.ClientToken = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            Logging.Info(this.GetType(), "Player is sending its notification token.");
        }
    }
}