namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;

    using State = ClashRoyale.Client.Logic.Enums.State;

    internal class PreLoginMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10100;
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

        private int Protocol        = 1;
        private int KeyVersion      = 15;

        private int Major           = 3;
        private int Minor           = 0;
        private int Revision        = 830;

        private string Masterhash   = "67bbb9df2eabf5c5fa4a8fa480df2a46bc861d87"; // 67bbb9df2eabf5c5fa4a8fa480df2a46bc861d87

        private int DeviceType      = 2;
        private int AppStore        = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreLoginMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="KeyVersion">The key version.</param>
        public PreLoginMessage(Device Device, int KeyVersion) : base(Device)
        {
            this.KeyVersion     = KeyVersion;
            this.Device.State   = State.SESSION;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteInt(this.Protocol);
            this.Stream.WriteInt(this.KeyVersion);

            this.Stream.WriteInt(this.Major);
            this.Stream.WriteInt(this.Minor);
            this.Stream.WriteInt(this.Revision);

            this.Stream.WriteString(this.Masterhash);

            this.Stream.WriteInt(this.DeviceType);
            this.Stream.WriteInt(this.AppStore);
        }
    }
}