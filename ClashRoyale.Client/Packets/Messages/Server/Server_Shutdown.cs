namespace ClashRoyale.Client.Packets.Messages.Server
{
    using ClashRoyale.Client.Logic;

    internal class Server_Shutdown : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server_Shutdown"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Server_Shutdown(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Server_Shutdown.
        }
    }
}