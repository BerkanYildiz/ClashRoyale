namespace ClashRoyale.Listener
{
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    public sealed class ServerGameListener : GameListener
    {
        internal Device Device;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServerGameListener"/> class.
        /// </summary>
        public ServerGameListener(Device device)
        {
            this.Device = device;
        }

        public override bool IsConnected()
        {
            return this.Device.Network.IsConnected;
        }

        public override bool IsAndroid()
        {
            return this.Device.Defines.Android;
        }

        public override void Matchmaking()
        {
            // TODO Implement Matchmaking.
        }

        public override void SendMessage(Message message)
        {
            if (this.Device.Network.IsConnected)
            {
                this.Device.NetworkManager.SendMessage(message);
            }
        }
    }
}