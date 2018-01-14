namespace ClashRoyale.Logic
{
    using ClashRoyale.Enums;
    using ClashRoyale.Listener;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Messages.Client.Account;
    using ClashRoyale.Network;

    using Timer = System.Timers.Timer;

    public class Device
    {
        public ClientGameListener   GameListener;
        public NetworkManager       NetworkManager;
        public NetworkToken         Token;
        public GameMode             GameMode;
        public Defines              Defines;
        public Timer                Timer;

        public State                State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device()
        {
            this.GameListener   = new ClientGameListener(this);
            this.NetworkManager = new NetworkManager(this);
            this.Defines        = new Defines();

            // Prepare..

            if (NetworkTcp.StartConnect(out this.Token))
            {
                this.Token.SetDevice(this);

                if (this.Token.IsConnected)
                {
                    this.NetworkManager.SendMessage(new ClientHelloMessage()
                    {
                        Protocol        = 1,
                        KeyVersion      = 15,
                        BuildVersion    = Config.ClientBuildVersion,
                        MajorVersion    = Config.ClientMajorVersion,
                        MinorVersion    = Config.ClientMinorVersion,
                        MasterHash      = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                        AppStore        = 2,
                        DeviceType      = 2
                    });

                    NetworkTcp.StartReceive(this.Token);
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