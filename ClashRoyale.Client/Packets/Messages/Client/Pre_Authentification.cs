namespace ClashRoyale.Client.Packets.Messages.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Logic.Enums;

    internal class Pre_Authentification : Message
    {
        internal int Protocol       = 1;
        internal int KeyVersion     = 14;

        internal int Major          = 3;
        internal int Minor          = 0;
        internal int Revision       = 830;

        internal string Masterhash  = "67bbb9df2eabf5c5fa4a8fa480df2a46bc861d87"; // 67bbb9df2eabf5c5fa4a8fa480df2a46bc861d87

        internal int DeviceType     = 2;
        internal int AppStore       = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pre_Authentification"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Pre_Authentification(Device Device, int KeyVersion) : base(Device)
        {
            this.Identifier     = 10100;
            this.KeyVersion     = KeyVersion;
            this.Device.State   = State.SESSION;
        }

        /// <summary>
        /// Encodes the <see cref="Message" />, using the <see cref="Writer" /> instance.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddInt(this.Protocol);
            this.Data.AddInt(this.KeyVersion);

            this.Data.AddInt(this.Major);
            this.Data.AddInt(this.Minor);
            this.Data.AddInt(this.Revision);

            this.Data.AddString(this.Masterhash);

            this.Data.AddInt(this.DeviceType);
            this.Data.AddInt(this.AppStore);
        }
    }
}