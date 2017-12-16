namespace ClashRoyale.Server.Network.Packets.Client.Account
{
    using System.Linq;

    using ClashRoyale.Server.Crypto;
    using ClashRoyale.Server.Crypto.Nacl;
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Files;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server.Account;

    internal class ClientHelloMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10100;
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

        private int AppStore;
        private int DeviceType;
        private int KeyVersion;
        private int Protocol;
        private int MajorVersion;
        private int MinorVersion;
        private int BuildVersion;

        private string MasterHash;

        internal const string ProdHost      = "gcroyale.gobelinland.fr";
        internal const string DevHost       = "";
        internal const string KunlunHost    = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelloMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ClientHelloMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            this.Device.State   = State.Session;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Protocol       = this.Stream.ReadInt();
            this.KeyVersion     = this.Stream.ReadInt();
            this.MajorVersion   = this.Stream.ReadInt();
            this.MinorVersion   = this.Stream.ReadInt();
            this.BuildVersion   = this.Stream.ReadInt();
            this.MasterHash     = this.Stream.ReadString();
            this.DeviceType     = this.Stream.ReadInt();
            this.AppStore       = this.Stream.ReadInt();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (this.Protocol == 1)
            {
                if (this.MajorVersion == Logic.Version.ClientMajorVersion && this.MinorVersion == 0 && this.BuildVersion == Logic.Version.ClientBuildVersion)
                {
                    if (string.Equals(this.MasterHash, Fingerprint.Masterhash))
                    {
                        if (PepperFactory.ServerSecretKeys.TryGetValue(this.KeyVersion, out byte[] SecretKey))
                        {
                            if (this.DeviceType == 3)
                            {
                                if (!Logic.Version.IsDev)
                                {
                                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Redirection)); // Dev Host

                                    return;
                                }
                                else
                                {
                                    if (this.KeyVersion != PepperFactory.ServerSecretKeys.Keys.Last())
                                    {
                                        this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Update));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (Logic.Version.IsDev)
                                {
                                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Redirection)); // Prod Host

                                    return;
                                }
                            }

                            if (this.DeviceType == 30)
                            {
                                if (!Logic.Version.IsKunlunServer)
                                {
                                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Redirection)); // Kunlun Host

                                    return;
                                }
                            }
                            else
                            {
                                if (Logic.Version.IsKunlunServer)
                                {
                                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Redirection)); // Prod Host
                                    return;
                                }
                            }

                            if (Program.Initialized)
                            {
                                this.Device.NetworkManager.PepperInit.KeyVersion = this.KeyVersion;
                                this.Device.NetworkManager.PepperInit.ServerPublicKey = new byte[32];

                                Curve25519Xsalsa20Poly1305.CryptoBoxGetpublickey(this.Device.NetworkManager.PepperInit.ServerPublicKey, SecretKey);

                                this.Device.NetworkManager.SendMessage(new ServerHelloMessage(this.Device));
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
                        this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Patch));
                    }
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Update));
                }
            }
            else
            {
                this.Device.NetworkManager.SendMessage(new AuthentificationFailedMessage(this.Device, Reason.Update));
            }
        }
    }
}