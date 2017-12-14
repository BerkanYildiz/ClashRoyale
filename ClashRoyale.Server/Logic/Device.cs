namespace ClashRoyale.Server.Logic
{
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network;

    internal class Device
    {
        internal NetworkManager NetworkManager;
        internal NetworkToken   Network;
        internal Player         Player;

        internal State State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="Network">The token.</param>
        internal Device(NetworkToken Network)
        {
            this.NetworkManager = new NetworkManager(this);
            this.Network        = Network;

            this.Network.SetDevice(this);
        }
    }
}
