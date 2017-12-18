namespace ClashRoyale.Client.Packets.Messages.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Logic.Enums;

    internal class Authentification : Message
    {
        internal int HighID             = 2;
        internal int LowID              = 14318;

        internal string Token           = "hw8r72xfkmnger9jrwntwkwha3ksas2ywrbcafwj";
        internal string MasterHash      = "67bbb9df2eabf5c5fa4a8fa480df2a46bc861d87";

        internal string AdvertiseID     = "00000000-0000-0000-0000-000000000000";
        internal string OpenUDID        = "00000000-0000-0000-0000-000000000000";

        internal int Major              = 3;
        internal int Minor              = 0;
        internal int Revision           = 830;

        internal string Model           = "iPhone9,3";
        internal string OSVersion       = "10.3.1";
        internal string Region          = "fr-FR";

        internal string MACAddress      = null;

        internal bool Android           = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentification"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Authentification(Device Device) : base(Device)
        {
            this.Identifier     = 10101;
            this.Device.State   = State.LOGIN;
            this.Version        = 3;
        }

        /// <summary>
        /// Encodes the <see cref="Message" />, using the <see cref="Writer" /> instance.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddInt(this.HighID);
            this.Data.AddInt(this.LowID);

            this.Data.AddString(this.Token);

            this.Data.AddVInt(this.Major);
            this.Data.AddVInt(this.Minor);
            this.Data.AddVInt(this.Revision);

            this.Data.AddString(this.MasterHash);

            this.Data.AddHexa("00-00-00-00-00-00-00-10-30-38-34-37-30-32-31-31-32-35-37-39-34-33-31-38-FF-FF-FF-FF-00-00-00-08-47-54-2D-50-35-32-31-30-00-00-00-24-61-62-33-35-30-33-32-34-2D-61-37-38-35-2D-34-61-39-32-2D-62-39-61-37-2D-66-61-35-61-37-66-32-34-36-62-32-32-00-00-00-05-34-2E-34-2E-34-01-00-00-00-00-00-00-00-10-30-38-34-37-30-32-31-31-32-35-37-39-34-33-31-38-00-00-00-05-66-72-2D-46-52-01-01-00-00-00-00-01-00-00-00-00-1D-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00");
            return;

            this.Data.AddString(null);

            this.Data.AddString(null); // Android ID
            this.Data.AddString(this.MACAddress);
            this.Data.AddString(this.Model);

            this.Data.AddVInt(0);

            this.Data.AddString(this.Region);
            this.Data.AddString(this.OpenUDID);
            this.Data.AddString(this.OSVersion);

            this.Data.AddBool(this.Android);

            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddString(null);

            this.Data.AddBool(false);

            this.Data.AddString(null);
        }
    }
}