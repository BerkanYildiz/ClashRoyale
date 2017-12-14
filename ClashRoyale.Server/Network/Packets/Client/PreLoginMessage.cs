namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Files;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class PreLoginMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10100;
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

        private int AppStore;
        private int DeviceType;
        private int KeyVersion;
        private int Protocol;

        private int MajorVersion;
        private int MinorVersion;
        private int BuildVersion;

        private string MasterHash;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreLoginMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public PreLoginMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // PreLoginMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
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
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.State = State.Session;

            if (this.MajorVersion != 3 || this.MinorVersion != 0 || this.BuildVersion != 830)
            {
                this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Update));
            }
            else
            {
                if (!string.Equals(this.MasterHash, Fingerprint.Masterhash))
                {
                    this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Patch));
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new PreLoginOkMessage(this.Device));
                }
            }
        }
    }
}