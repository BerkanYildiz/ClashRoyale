namespace ClashRoyale.Proxy.Network.States
{
    using System.Net.Sockets;

    public class ServerState : State
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerState"/> class.
        /// </summary>
        /// <param name="Socket">The socket.</param>
        public ServerState(Socket Socket) : base()
        {
            this.Socket = Socket;
        }

        public byte[] ClientKey;
        public byte[] Nonce;
        public byte[] SessionKey;
        public byte[] SharedKey;
    }
}