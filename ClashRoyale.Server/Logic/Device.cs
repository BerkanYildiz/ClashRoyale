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

        public ServerGameListener   GameListener;
        public NetworkManager       NetworkManager;
        public NetworkToken         Token;
        public Defines              Defines;
        public GameMode             GameMode;

        public State State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="Token">The token.</param>
        public Device(NetworkToken Token)
        {
            this.NetworkManager = new NetworkManager(this);
            this.GameListener   = new ServerGameListener(this);
            this.Token          = Token;
            this.Token.SetDevice(this);
        }
    }
}
