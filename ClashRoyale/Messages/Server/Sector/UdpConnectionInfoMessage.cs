namespace ClashRoyale.Messages.Server.Sector
{
    using ClashRoyale.Enums;

    public class UdpConnectionInfoMessage : Message
    {
        internal int ServerPort;
        internal string Nonce;
        internal string ServerHost;
        internal byte[] SessionId;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24112;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Sector;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpConnectionInfoMessage"/> class.
        /// </summary>
        public UdpConnectionInfoMessage()
        {
            // UdpConnectionInfoMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ServerPort = this.Stream.ReadVInt();
            this.ServerHost = this.Stream.ReadString();
            this.SessionId = this.Stream.ReadBytes();
            this.Nonce = this.Stream.ReadStringReference();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.ServerPort);
            this.Stream.WriteString(this.ServerHost);
            this.Stream.WriteBytes(this.SessionId);
            this.Stream.WriteStringReference(this.Nonce);
        }
    }
}