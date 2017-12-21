namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Player;

    internal class AuthentificationOkMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 22280;
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

        private readonly Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthentificationOkMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="PassToken">The pass token.</param>
        public AuthentificationOkMessage(Device Device, Player Player) : base(Device)
        {
            this.Version = 1;
            this.Player = Player;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteLong(this.Player.PlayerId);
            this.Stream.WriteLong(this.Player.PlayerId);

            this.Stream.WriteString(this.Player.Token);

            this.Stream.WriteString(null); // Facebook
            this.Stream.WriteString(null); // Gamecenter

            this.Stream.WriteVInt(Config.ServerMajorVersion);
            this.Stream.WriteVInt(Config.ServerBuildVersion);
            this.Stream.WriteVInt(Config.ServerBuildVersion);
            this.Stream.WriteVInt(Config.ServerMinorVersion);

            this.Stream.WriteString(Config.Environment);

            // B6-17  96-9A-41  8B-0B

            this.Stream.WriteVInt(0); // Session Count
            this.Stream.WriteVInt(0); // PlayTime Seconds.
            this.Stream.WriteVInt(0); // Days Since Started Playing.

            this.Stream.WriteString("815255971920210"); // Facebook App Id
            this.Stream.WriteString("1507593815116");
            this.Stream.WriteString("1507593409000");

            this.Stream.WriteVInt(0);

            this.Stream.WriteString("103419370274411296720");
            this.Stream.WriteString(null);
            this.Stream.WriteString(null);

            this.Stream.WriteString("FR"); // Country - Pays
            this.Stream.WriteString("Melun"); // City - Ville
            this.Stream.WriteString("A8"); // State - Region

            this.Stream.WriteVInt(2);
            {
                this.Stream.AddRange("91-A1-3B".HexaToBytes());
                this.Stream.AddRange("A8-9E-03".HexaToBytes());
            }

            this.Stream.WriteVInt(2);
            {
                this.Stream.WriteString("https://game-assets.clashroyaleapp.com");
                this.Stream.WriteString("https://99faf1e355c749a9a049-2a63f4436c967aa7d355061bd0d924a1.ssl.cf1.rackcdn.com");
            }

            this.Stream.WriteVInt(1);
            {
                this.Stream.WriteString("https://event-assets.clashroyale.com");
            }
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.State = State.Logged;
        }
    }
}