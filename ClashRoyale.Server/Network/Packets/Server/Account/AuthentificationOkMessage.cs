namespace ClashRoyale.Server.Network.Packets.Server.Account
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class AuthentificationOkMessage : Message
    {
        internal string PassToken;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 28502;
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
        /// Initializes a new instance of the <see cref="AuthentificationOkMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="PassToken">The pass token.</param>
        public AuthentificationOkMessage(Device Device, string PassToken) : base(Device)
        {
            this.Version    = 1;
            this.PassToken  = PassToken;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            /* 
             * 00-00-00-02
             * 00-00-37-EE
             * 00-00-00-02
             * 00-00-37-EE
             * 
             * 00-00-00-28  68-77-38-72-37-32-78-66-6B-6D-6E-67-65-72-39-6A-72-77-6E-74-77-6B-77-68-61-33-6B-73-61-73-32-79-77-72-62-63-61-66-77-6A
             * 00-00-00-0F  32-31-38-37-35-35-36-38-31-37-39-38-38-30-35
             * 00-00-00-0B  47-3A-33-32-35-33-37-38-36-37-31
             * 
             * 03  BE-0C  BE-0C  05
             * 
             * 00-00-00-04  70-72-6F-64
             * 
             * A0-17-80-91-41-83-0B
             * 
             * 00-00-00-10  31-34-37-35-32-36-38-37-38-36-31-31-32-34-33-33
             * 00-00-00-0D  31-35-31-33-31-30-38-33-38-36-32-30-37
             * 00-00-00-0D  31-34-35-31-39-39-36-37-37-38-30-30-30
             * 
             * 00
             * 
             * 00-00-00-15  31-30-33-34-31-39-33-37-30-32-37-34-34-31-31-32-39-36-37-32-30
             * 
             * FF-FF-FF-FF
             * FF-FF-FF-FF
             * 
             * 00-00-00-02  46-52
             * 00-00-00-05  4D-65-6C-75-6E
             * 00-00-00-02  41-38
             * 
             * 02-91-A1-3B-A8-9E-03-02
             * 
             * 00-00-00-26  68-74-74-70-73-3A-2F-2F-67-61-6D-65-2D-61-73-73-65-74-73-2E-63-6C-61-73-68-72-6F-79-61-6C-65-61-70-70-2E-63-6F-6D
             * 00-00-00-51  68-74-74-70-73-3A-2F-2F-39-39-66-61-66-31-65-33-35-35-63-37-34-39-61-39-61-30-34-39-2D-32-61-36-33-66-34-34-33-36-63-39-36-37-61-61-37-64-33-35-35-30-36-31-62-64-30-64-39-32-34-61-31-2E-73-73-6C-2E-63-66-31-2E-72-61-63-6B-63-64-6E-2E-63-6F-6D
             * 
             * 01
             * 
             * 00-00-00-24  68-74-74-70-73-3A-2F-2F-65-76-65-6E-74-2D-61-73-73-65-74-73-2E-63-6C-61-73-68-72-6F-79-61-6C-65-2E-63-6F-6D
             */

            this.Stream.WriteLong(this.Device.NetworkManager.AccountId);
            this.Stream.WriteLong(this.Device.NetworkManager.AccountId);

            this.Stream.WriteString(this.PassToken);

            // this.Stream.WriteBytes("00-00-00-0F-32-31-38-37-35-35-36-38-31-37-39-38-38-30-35-00-00-00-0B-47-3A-33-32-35-33-37-38-36-37-31-03-BE-0C-BE-0C-05-00-00-00-04-70-72-6F-64-A1-17-BD-91-41-83-0B-00-00-00-10-31-34-37-35-32-36-38-37-38-36-31-31-32-34-33-33-00-00-00-0D-31-35-31-33-31-30-38-39-36-35-37-34-31-00-00-00-0D-31-34-35-31-39-39-36-37-37-38-30-30-30-00-00-00-00-15-31-30-33-34-31-39-33-37-30-32-37-34-34-31-31-32-39-36-37-32-30-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-02-46-52-00-00-00-05-4D-65-6C-75-6E-00-00-00-02-41-38-02-91-A1-3B-A8-9E-03-02-00-00-00-26-68-74-74-70-73-3A-2F-2F-67-61-6D-65-2D-61-73-73-65-74-73-2E-63-6C-61-73-68-72-6F-79-61-6C-65-61-70-70-2E-63-6F-6D-00-00-00-51-68-74-74-70-73-3A-2F-2F-39-39-66-61-66-31-65-33-35-35-63-37-34-39-61-39-61-30-34-39-2D-32-61-36-33-66-34-34-33-36-63-39-36-37-61-61-37-64-33-35-35-30-36-31-62-64-30-64-39-32-34-61-31-2E-73-73-6C-2E-63-66-31-2E-72-61-63-6B-63-64-6E-2E-63-6F-6D-01-00-00-00-24-68-74-74-70-73-3A-2F-2F-65-76-65-6E-74-2D-61-73-73-65-74-73-2E-63-6C-61-73-68-72-6F-79-61-6C-65-2E-63-6F-6D".HexaToBytes());

            // return;

            this.Stream.WriteString(this.Device.GameMode.Player.ApiManager.Facebook.Identifier);
            this.Stream.WriteString(this.Device.GameMode.Player.ApiManager.Gamecenter.Identifier);

            this.Stream.WriteVInt(Logic.Version.ServerMajorVersion);
            this.Stream.WriteVInt(Logic.Version.ServerBuildVersion);
            this.Stream.WriteVInt(Logic.Version.ServerBuildVersion);
            this.Stream.WriteVInt(Logic.Version.ServerMinorVersion);

            this.Stream.WriteString(Logic.Version.Environment);

            this.Stream.WriteVInt(0); // Session Count
            this.Stream.WriteVInt(0); // PlayTime Seconds.
            this.Stream.WriteVInt(0); // Days Since Started Playing.

            this.Stream.WriteString("815255971920210");
            this.Stream.WriteString("1507593815116");
            this.Stream.WriteString("1507593409000");

            this.Stream.WriteVInt(0);

            this.Stream.WriteString(null);
            this.Stream.WriteString(null);
            this.Stream.WriteString(null);

            this.Stream.WriteString("CA"); // Country - Pays
            this.Stream.WriteString("Port Coquitlam"); // City - Ville
            this.Stream.WriteString("BC"); // State - Region

            this.Stream.WriteVInt(1);

            this.Stream.WriteString("https://game-assets.clashroyaleapp.com");
            this.Stream.WriteString("https://99faf1e355c749a9a049-2a63f4436c967aa7d355061bd0d924a1.ssl.cf1.rackcdn.com");
            this.Stream.WriteString("https://event-assets.clashroyale.com");
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