namespace ClashRoyale.Messages.Client.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Messages;

    public class ClientHelloMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10100;
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

        public int AppStore;
        public int DeviceType;
        public int KeyVersion;
        public int Protocol;
        public int MajorVersion;
        public int MinorVersion;
        public int BuildVersion;

        public string MasterHash;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelloMessage"/> class.
        /// </summary>
        public ClientHelloMessage()
        {
            // ClientHelloMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelloMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ClientHelloMessage(ByteStream Stream) : base(Stream)
        {
            // ClientHelloMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Protocol       = this.Stream.ReadInt();
            this.KeyVersion     = this.Stream.ReadInt();
            this.MajorVersion   = this.Stream.ReadInt();
            this.MinorVersion   = this.Stream.ReadInt();
            this.BuildVersion   = this.Stream.ReadInt();

            this.MasterHash     = this.Stream.ReadString();

            this.DeviceType     = this.Stream.ReadInt();
            this.AppStore       = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.Protocol);
            this.Stream.WriteInt(this.KeyVersion);
            this.Stream.WriteInt(this.MajorVersion);
            this.Stream.WriteInt(this.MinorVersion);
            this.Stream.WriteInt(this.BuildVersion);

            this.Stream.WriteString(this.MasterHash);

            this.Stream.WriteInt(this.DeviceType);
            this.Stream.WriteInt(this.AppStore);
        }
    }
}