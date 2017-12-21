namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class RequestApiMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 15080;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestApiMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public RequestApiMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // RequestApiMessage.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.SendMessage(new RequestApiDataMessage(this.Device));
        }
    }
}