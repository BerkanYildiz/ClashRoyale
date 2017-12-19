namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class KeepAliveMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 19911;
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
        /// Initializes a new instance of the <see cref="KeepAliveMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public KeepAliveMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // KeepAliveMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.KeepAliveMessageReceived();
            // this.Device.NetworkManager.SendMessage(new KeepAliveServerMessage(this.Device));
        }
    }
}