namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class ClientCapabilitiesMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 11688;
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
        public override void Decode()
        {
            this.Ping       = this.Stream.ReadVInt();
            this.Interface  = this.Stream.ReadString();
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Ping);
            this.Stream.WriteString(this.Interface);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            this.Device.NetworkManager.Ping = this.Ping;
            this.Device.NetworkManager.ConnectionInterface = this.Interface;
        }
    }
}