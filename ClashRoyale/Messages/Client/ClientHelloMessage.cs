namespace ClashRoyale.Messages.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
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

        private int AppStore;
        private int DeviceType;
        private int KeyVersion;
        private int Protocol;
        private int MajorVersion;
        private int MinorVersion;
        private int BuildVersion;

        private string MasterHash;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelloMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ClientHelloMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            this.Device.State   = State.Session;
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