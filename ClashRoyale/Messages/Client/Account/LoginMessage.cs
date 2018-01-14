namespace ClashRoyale.Messages.Client.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv.Client;
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

        public int MajorVersion;
        public int MinorVersion;
        public int BuildVersion;

        public string OpenUdid;
        public string MacAddress;
        public string Model;

        public string AdvertiseId;
        public string AndroidId;
        public string OsVersion;
        public string Region;

        public bool IsAndroid;

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
        /// <param name="Stream">The stream.</param>
        public LoginMessage(ByteStream Stream) : base(Stream)
        {
            // LoginMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.HighId             = this.Stream.ReadInt();
            this.LowId              = this.Stream.ReadInt();
            this.Token              = this.Stream.ReadString();
            
            this.MajorVersion       = this.Stream.ReadVInt();
            this.MinorVersion       = this.Stream.ReadVInt();
            this.BuildVersion       = this.Stream.ReadVInt();

            this.MasterHash         = this.Stream.ReadString();

            this.Stream.ReadInt();

            this.OpenUdid           = this.Stream.ReadString();
            this.MacAddress         = this.Stream.ReadString();
            this.Model              = this.Stream.ReadString();

            this.AdvertiseId        = this.Stream.ReadString();
            this.OsVersion          = this.Stream.ReadString();

            this.IsAndroid          = this.Stream.ReadBoolean();

            this.Stream.ReadStringReference();

            this.AndroidId          = this.Stream.ReadStringReference();

            this.Region             = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.HighId);
            this.Stream.WriteInt(this.LowId);

            this.Stream.WriteString(this.Token);

            this.Stream.WriteVInt(this.MajorVersion);
            this.Stream.WriteVInt(this.MinorVersion);
            this.Stream.WriteVInt(this.BuildVersion);

            this.Stream.WriteString(this.MasterHash);

            this.Stream.WriteInt(0);

            this.Stream.WriteString(this.OpenUdid);
            this.Stream.WriteString(this.MacAddress);
            this.Stream.WriteString(this.Model);

            this.Stream.WriteString(this.AdvertiseId);
            this.Stream.WriteString(this.OsVersion);

            this.Stream.WriteBoolean(this.IsAndroid);

            this.Stream.WriteStringReference(string.Empty);
            this.Stream.WriteStringReference(this.AndroidId);

            this.Stream.WriteString(this.Region);
        }
    }
}