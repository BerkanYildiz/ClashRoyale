namespace ClashRoyale.Server.Network.Packets.Client.Account
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class ClientCapabilitiesMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10107;
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

        private int Ping;
        private string Interface;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCapabilitiesMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ClientCapabilitiesMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // ClientCapabilitiesMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Ping = this.Stream.ReadVInt();
            this.Interface = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.Ping = this.Ping;
            this.Device.NetworkManager.ConnectionInterface = this.Interface;
        }
    }
}