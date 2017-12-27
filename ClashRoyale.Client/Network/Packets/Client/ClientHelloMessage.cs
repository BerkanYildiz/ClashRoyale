namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;

    internal class ClientHelloMessage : Message
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

        private string Masterhash   = "74ecd0057e94aee0f6b485473ef3a047b4663e39"; // 74ecd0057e94aee0f6b485473ef3a047b4663e39

        private int DeviceType      = 2;
        private int AppStore        = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelloMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        public ClientHelloMessage(Bot Bot) : base(Bot)
        {
            this.KeyVersion = 15;
            this.Bot.State  = State.Session;
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