namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class ServerHelloMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20100;
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

        public byte[] SessionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHelloMessage"/> class.
        /// </summary>
        public ServerHelloMessage()
        {
            // ServerHelloMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHelloMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ServerHelloMessage(ByteStream Stream) : base(Stream)
        {
            // ServerHelloMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHelloMessage"/> class.
        /// </summary>
        /// <param name="SessionKey">The session key.</param>
        public ServerHelloMessage(byte[] SessionKey)
        {
            this.SessionKey = SessionKey;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.SessionKey = this.Stream.ReadBytes();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBytes(this.SessionKey);
        }
    }
}