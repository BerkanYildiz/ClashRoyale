namespace ClashRoyale.Proxy.Network
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using ClashRoyale.Proxy.Logic;
    using ClashRoyale.Proxy.Logic.Collections;

    public class Gateway
    {
        private readonly Socket Server;
        private readonly Thread TCPThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class.
        /// </summary>
        public Gateway()
        {
            this.TCPThread  = new Thread(this.ListenTCP);
            this.Server     = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.Server.Bind(new IPEndPoint(IPAddress.Any, 9339));
            this.Server.Listen(100);

            this.TCPThread.Start();
        }

        private void ListenTCP()
        {
            while (true)
            {
                Socket Socket = this.Server.Accept();

                if (Socket.Connected && (Socket.RemoteEndPoint.ToString().StartsWith("192.168.")))
                {
                    Devices.Add(new Device(Socket));
                }
            }
        }
    }
}