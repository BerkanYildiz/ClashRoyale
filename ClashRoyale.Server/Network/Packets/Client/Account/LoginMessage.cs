namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Files;
    using ClashRoyale.Files.Csv.Client;
    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class LoginMessage : Message
    {
        private int HighId;
        private int LowId;

        private string Token;
        private string MasterHash;
        private string SVersion;

        private int MajorVersion;
        private int MinorVersion;
        private int BuildVersion;

        private LocaleData Locale;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public LoginMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            this.Device.State       = State.Login;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.HighId             = this.Stream.ReadInt();
            this.LowId              = this.Stream.ReadInt();
            this.Token              = this.Stream.ReadString();

            this.MajorVersion       = this.Stream.ReadVInt();
            this.MinorVersion       = this.Stream.ReadVInt();
            this.BuildVersion       = this.Stream.ReadVInt();

            this.MasterHash         = this.Stream.ReadString();

            this.Stream.ReadInt();

            this.Device.Defines.OpenUdid    = this.Stream.ReadString();
            this.Device.Defines.MacAddress  = this.Stream.ReadString();
            this.Device.Defines.Model       = this.Stream.ReadString();

            this.Device.Defines.AdvertiseId = this.Stream.ReadString();
            this.Device.Defines.OsVersion   = this.Stream.ReadString();

            this.Device.Defines.Android     = this.Stream.ReadBoolean();

            this.Stream.ReadStringReference();

            this.Device.Defines.AndroidId   = this.Stream.ReadStringReference();

            this.Device.Defines.Region  = this.Stream.ReadString();
        }

        /// <summary>
        /// Returns whether we should handle this device or nah.
        /// </summary>
        internal bool Trusted()
        {
            if (this.ValuesAreCorrect())
            {
                if (this.MajorVersion == Logic.Version.ClientMajorVersion && this.MinorVersion == 0 && this.BuildVersion == Logic.Version.ClientBuildVersion)
                {
                    if (Program.Initialized)
                    {
                        if (string.Equals(this.MasterHash, Fingerprint.Masterhash))
                        {
                            return true;
                        }
                        else
                        {
                            this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Patch));
                        }
                    }
                    else
                    {
                        this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Maintenance));
                    }
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Update));
                }
            }
            else
            {
                TcpGateway.Disconnect(this.Device.Network.AsyncEvent);
            }

            return false;
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override async void Process()
        {
            if (!this.Trusted())
            {
                return;
            }

            Logging.Info(this.GetType(), "Account Id    : " + this.HighId + "-" + this.LowId + ".");
            Logging.Info(this.GetType(), "Account Token : " + this.Token + ".");

            if (this.HighId == 0 && this.LowId == 0 && this.Token == null)
            {
                Player Player = await Players.Create();
                
                if (Player != null)
                {
                    await this.Login(Player);
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Maintenance));
                }
            }
            else
            {
                Player Player = await Players.Get(this.HighId, this.LowId);

                if (Player != null)
                {
                    if (string.Equals(this.Token, Player.Token))
                    {
                        if (!Player.IsBanned)
                        {
                            if (Player.IsConnected)
                            {
                                this.Device.NetworkManager.SendMessage(new DisconnectedMessage(Player.GameMode.Device));
                            }

                            await this.Login(Player);
                        }
                        else
                        {
                            this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Banned));
                        }
                    }
                    else
                    {
                        this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Reset));
                    }
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Reset));
                }
            }
        }

        /// <summary>
        /// Checks if values are correct.
        /// </summary>
        /// <returns>A bool indicating whether the values are correct or no.</returns>
        internal bool ValuesAreCorrect()
        {
            if (this.HighId >= 0)
            {
                if (this.LowId >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        internal async Task Login(Player Player)
        {
            if (Player.AccountLocation == null)
            {
                Player.AccountLocation = this.Locale;
            }

            this.Device.GameMode = new GameMode(this.Device);
            this.Device.GameMode.SetPlayer(Player);
            this.Device.NetworkManager.AccountId = new LogicLong(Player.HighId, Player.LowId);

            this.Device.NetworkManager.SendMessage(new AuthentificationOkMessage(this.Device, Player));
            this.Device.NetworkManager.SendMessage(new OwnHomeDataMessage(this.Device, Player));
            this.Device.NetworkManager.SendMessage(new InboxCountMessage(this.Device));

            /* if (Player.IsInAlliance)
            {
                Clan Clan = await Resources.Alliances.Get(Player.ClanHighId, Player.ClanLowId);

                if (Clan != null)
                {
                    if (await Clan.Members.AddOnlinePlayer(Player))
                    {
                        new AllianceStreamMessage(this.Device, Clan.Messages.ToArray()));
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "AddOnlinePlayer(Player) != true at Login(Player).");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Clan == null at Login(Player).");
                }
            } */
        }
    }
}