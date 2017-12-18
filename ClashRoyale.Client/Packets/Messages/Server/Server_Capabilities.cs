namespace ClashRoyale.Client.Packets.Messages.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Packets.Messages.Client;

    internal class Server_Capabilities : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server_Capabilities"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Server_Capabilities(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Server_Capabilities.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.Client.Gateway.Send(new Client_Capabilities(this.Device));
        }
    }
}