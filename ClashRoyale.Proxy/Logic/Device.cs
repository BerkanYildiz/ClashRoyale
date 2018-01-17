namespace ClashRoyale.Logic
{
    using ClashRoyale.Enums;
    using ClashRoyale.Listener;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Network;

    public class Device
    {
        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        public long DeviceId
        {
            get;
            set;
        }

        public GameListener         GameListener;
        public NetworkManager       NetworkManager;
        public NetworkToken         Token;
        public Defines              Defines;
        public GameMode             GameMode;

        public State State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device()
        {
            this.GameListener   = new ClientGameListener(this);
            this.NetworkManager = new NetworkManager(this);
            this.Defines        = new Defines();

            // Prepare..
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="Token">The token.</param>
        public Device(NetworkToken Token)
        {
            this.NetworkManager = new NetworkManager(this);
            this.Token          = Token;
            this.Token.SetDevice(this);

            Device Server       = new Device();

            this.GameListener   = new ServerGameListener(Server);
            Server.GameListener = new ClientGameListener(this);

            Server.Connect("game.clashroyaleapp.com");
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect(string Host)
        {
            if (NetworkTcp.StartConnect(Host, out this.Token))
            {
                this.Token.SetDevice(this);

                if (this.Token.IsConnected)
                {
                    /* this.NetworkManager.SendMessage(new ClientHelloMessage()
                    {
                        Protocol        = 1,
                        KeyVersion      = 15,
                        BuildVersion    = Config.ClientBuildVersion,
                        MajorVersion    = Config.ClientMajorVersion,
                        MinorVersion    = Config.ClientMinorVersion,
                        MasterHash      = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                        AppStore        = 2,
                        DeviceType      = 2
                    }); */
                }
                else
                {
                    Logging.Warning(this.GetType(), "Token.IsConnected == false at Device().");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "StartConnect(out this.Token) == false at Device().");
            }
        }
    }
}
