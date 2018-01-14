namespace ClashRoyale.Messages.Client.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Messages;

    public class ClientCapabilitiesMessage : Message
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

        public int Ping;
        public string Interface;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCapabilitiesMessage"/> class.
        /// </summary>
        public ClientCapabilitiesMessage()
        {
            // ClientCapabilitiesMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCapabilitiesMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ClientCapabilitiesMessage(ByteStream Stream) : base(Stream)
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
    }
}