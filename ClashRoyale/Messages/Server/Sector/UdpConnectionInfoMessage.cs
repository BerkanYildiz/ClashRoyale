namespace ClashRoyale.Messages.Server.Sector
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class UdpConnectionInfoMessage : Message
    {
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

        public int ServerPort;
        public string Nonce;
        public string ServerHost;
        public byte[] SessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpConnectionInfoMessage"/> class.
        /// </summary>
        public UdpConnectionInfoMessage()
        {
            // UdpConnectionInfoMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpConnectionInfoMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public UdpConnectionInfoMessage(ByteStream Stream) : base(Stream)
        {
            // UdpConnectionInfoMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpConnectionInfoMessage"/> class.
        /// </summary>
        /// <param name="ServerPort">The server port.</param>
        /// <param name="ServerHost">The server host.</param>
        /// <param name="SessionId">The session identifier.</param>
        /// <param name="Nonce">The nonce.</param>
        public UdpConnectionInfoMessage(int ServerPort, string ServerHost, byte[] SessionId, string Nonce)
        {
            this.ServerPort = ServerPort;
            this.ServerHost = ServerHost;
            this.SessionId  = SessionId;
            this.Nonce      = Nonce;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ServerPort = this.Stream.ReadVInt();
            this.ServerHost = this.Stream.ReadString();
            this.SessionId  = this.Stream.ReadBytes();
            this.Nonce      = this.Stream.ReadStringReference();
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