namespace ClashRoyale.Logic
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Network;

    public class Device
    {
        public NetworkManager NetworkManager;
        public NetworkToken   Network;
        public Defines        Defines;
        public GameMode       GameMode;

        public State State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="Network">The token.</param>
        public Device(NetworkToken Network)
        {
            this.NetworkManager = new NetworkManager(this);
            this.Network        = Network;
            this.Network.SetDevice(this);
        }
    }
}
