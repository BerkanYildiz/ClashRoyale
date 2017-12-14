namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Files;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class LoginMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10101;
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

        private int HighId;
        private int LowId;

        private string Token;
        private string MasterHash;

        private int MajorVersion;
        private int MinorVersion;
        private int BuildVersion;

        private string OpenUdid;
        private string AdvertiseId;
        private string MacAddress;
        private string AndroidId;
        private string Region;

        private string PhoneModel;
        private string PhoneVersion;

        private bool IsAndroid;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public LoginMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            this.Device.State   = State.Login;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.HighId         = this.Stream.ReadInt();
            this.LowId          = this.Stream.ReadInt();
            this.Token          = this.Stream.ReadString();

            this.MajorVersion   = this.Stream.ReadVInt();
            this.MinorVersion   = this.Stream.ReadVInt();
            this.BuildVersion   = this.Stream.ReadVInt();

            this.MasterHash     = this.Stream.ReadString();

            this.Stream.ReadInt();

            this.OpenUdid       = this.Stream.ReadString();
            this.MacAddress     = this.Stream.ReadString();
            this.PhoneModel     = this.Stream.ReadString();

            this.AdvertiseId    = this.Stream.ReadString();
            this.PhoneVersion   = this.Stream.ReadString();

            this.IsAndroid      = this.Stream.ReadBoolean();

            this.Stream.ReadStringReference();
            
            this.AndroidId      = this.Stream.ReadStringReference();
            this.Region         = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override async void Process()
        {
            if (this.MajorVersion != Config.ClientMajorVersion || this.MinorVersion != Config.ClientMinorVersion || this.BuildVersion != Config.ClientBuildVersion)
            {
                this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Update));
            }
            else
            {
                if (!string.Equals(this.MasterHash, Fingerprint.Masterhash))
                {
                    this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Patch));
                }
            }

            if (this.Device.State == State.Login)
            {
                if (this.HighId == 0 && this.LowId == 0)
                {
                    Player Player = await Players.Create();

                    if (Player != null)
                    {
                        this.Login(Player);
                    }
                    else
                    {
                        this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Maintenance));
                    }
                }
                else
                {
                    Player Player = await Players.Get(this.HighId, this.LowId);

                    if (Player != null)
                    {
                        if (string.Equals(this.Token, Player.Token))
                        {
                            this.Login(Player);
                        }
                        else
                        {
                            this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Reset));
                        }
                    }
                    else
                    {
                        this.Device.NetworkManager.SendMessage(new LoginFailedMessage(this.Device, Reason.Reset));
                    }
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "State != Login at Process().");
            }
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        private void Login(Player Player)
        {
            this.Device.Player = Player;

            this.Device.NetworkManager.SendMessage(new LoginOkMessage(this.Device));
            this.Device.NetworkManager.SendMessage(new OwnHomeDataMessage(this.Device));
        }
    }
}