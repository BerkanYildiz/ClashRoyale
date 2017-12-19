namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    using State = ClashRoyale.Client.Logic.Enums.State;

    internal class LoginMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10101;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        private int HighId              = 2;
        private int LowId               = 14318;

        private string Token            = "hw8r72xfkmnger9jrwntwkwha3ksas2ywrbcafwj";
        private string MasterHash       = "67bbb9df2eabf5c5fa4a8fa480df2a46bc861d87";

        private string AdvertiseId      = "00000000-0000-0000-0000-000000000000";
        private string OpenUdid         = "00000000-0000-0000-0000-000000000000";

        private int Major               = 3;
        private int Minor               = 0;
        private int Revision            = 830;

        private string Model            = "iPhone9,3";
        private string OsVersion        = "10.3.1";
        private string Region           = "fr-FR";

        private string MacAddress       = null;

        private bool Android            = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public LoginMessage(Device Device) : base(Device)
        {
            this.Device.State   = State.LOGIN;
            this.Version        = 3;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteInt(this.HighId);
            this.Stream.WriteInt(this.LowId);

            this.Stream.WriteString(this.Token);

            this.Stream.WriteVInt(this.Major);
            this.Stream.WriteVInt(this.Minor);
            this.Stream.WriteVInt(this.Revision);

            this.Stream.WriteString(this.MasterHash);

            this.Stream.AddRange("00-00-00-00-00-00-00-10-30-38-34-37-30-32-31-31-32-35-37-39-34-33-31-38-FF-FF-FF-FF-00-00-00-08-47-54-2D-50-35-32-31-30-00-00-00-24-61-62-33-35-30-33-32-34-2D-61-37-38-35-2D-34-61-39-32-2D-62-39-61-37-2D-66-61-35-61-37-66-32-34-36-62-32-32-00-00-00-05-34-2E-34-2E-34-01-00-00-00-00-00-00-00-10-30-38-34-37-30-32-31-31-32-35-37-39-34-33-31-38-00-00-00-05-66-72-2D-46-52-01-01-00-00-00-00-01-00-00-00-00-1D-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".HexaToBytes());
            return;

            this.Stream.WriteString(null);

            this.Stream.WriteString(null); // Android ID
            this.Stream.WriteString(this.MacAddress);
            this.Stream.WriteString(this.Model);

            this.Stream.WriteVInt(0);

            this.Stream.WriteString(this.Region);
            this.Stream.WriteString(this.OpenUdid);
            this.Stream.WriteString(this.OsVersion);

            this.Stream.WriteBoolean(this.Android);

            this.Stream.WriteString(null);
            this.Stream.WriteString(null);
            this.Stream.WriteString(null);

            this.Stream.WriteBoolean(false);

            this.Stream.WriteString(null);
        }
    }
}