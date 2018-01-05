namespace ClashRoyale.Messages.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv.Client;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    public class LoginMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10101;
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

        public int HighId;
        public int LowId;

        public string Token;
        public string MasterHash;
        public string SVersion;

        public int MajorVersion;
        public int MinorVersion;
        public int BuildVersion;

        public LocaleData Locale;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        public LoginMessage()
        {
            // LoginMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public LoginMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            this.Device.State               = State.Login;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.HighId                     = this.Stream.ReadInt();
            this.LowId                      = this.Stream.ReadInt();
            this.Token                      = this.Stream.ReadString();
            
            this.MajorVersion               = this.Stream.ReadVInt();
            this.MinorVersion               = this.Stream.ReadVInt();
            this.BuildVersion               = this.Stream.ReadVInt();

            this.MasterHash                 = this.Stream.ReadString();

            this.Stream.ReadInt();

            this.Device.Defines.OpenUdid    = this.Stream.ReadString();
            this.Device.Defines.MacAddress  = this.Stream.ReadString();
            this.Device.Defines.Model       = this.Stream.ReadString();

            this.Device.Defines.AdvertiseId = this.Stream.ReadString();
            this.Device.Defines.OsVersion   = this.Stream.ReadString();

            this.Device.Defines.Android     = this.Stream.ReadBoolean();

            this.Stream.ReadStringReference();

            this.Device.Defines.AndroidId   = this.Stream.ReadStringReference();

            this.Device.Defines.Region      = this.Stream.ReadString();
        }
    }
}