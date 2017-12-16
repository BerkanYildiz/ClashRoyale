namespace ClashRoyale.Server.Network.Packets.Server.Account
{
    using ClashRoyale.Server.Files;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class AuthentificationFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20103;
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
        
        /// <summary>
        /// Gets the patching host.
        /// </summary>
        internal string ContentUrl
        {
            get
            {
                return Fingerprint.IsCustom ? "https://raw.githubusercontent.com/BerkanYildiz/GL.Patchs/master/ClashRoyale" : "http://7166046b142482e67b30-2a63f4436c967aa7d355061bd0d924a1.r65.cf1.rackcdn.com";
            }
        }

        internal string UpdateUrl       = "https://mega.nz/#!E90UwDKa!e6Xv7bEXWSJDCyQy2o1PNqxONh0Q4qt3_rM5cks-pMo";
        internal string AssetsUrl       = "https://game-assets.clashroyaleapp.com";
        internal string RedirectDomain  = "88.189.197.11";
        internal string Message         = "GobelinLand";

        internal int TimeLeft;

        internal Reason Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthentificationFailedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Reason">The reason.</param>
        public AuthentificationFailedMessage(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            this.Version        = 4;
            this.Reason         = Reason;

            switch (Reason)
            {
                case Reason.Banned:
                {
                    this.TimeLeft = this.Device.GameMode.Player.BanSecondsLeft;
                    break;
                }

                case Reason.Maintenance:
                {
                    this.TimeLeft = 0;
                    break;
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt((int) this.Reason);

            this.Stream.WriteString(this.Reason == Reason.Patch ? Fingerprint.Json.ToString() : null);
            this.Stream.WriteString(this.UpdateUrl);
            this.Stream.WriteString(this.Message);      // V1
            this.Stream.WriteString(this.ContentUrl);   // V2
            this.Stream.WriteVInt(this.TimeLeft);
            this.Stream.WriteBoolean(false);            // V3

            this.Stream.WriteString(null);              // V4

            // Url

            this.Stream.WriteVInt(2); // Count
            {
                this.Stream.WriteString(this.AssetsUrl);
                this.Stream.WriteString(this.ContentUrl);
            }
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.State = State.Disconnected;
        }
    }
}