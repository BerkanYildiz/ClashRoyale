namespace ClashRoyale.Proxy.Network.States
{
    using System.Net.Sockets;

    public class ClientState : State
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientState"/> class.
        /// </summary>
        /// <param name="Socket">The socket.</param>
        public ClientState(Socket Socket) : base()
        {
            this.Socket = Socket;
        }

        public byte[] Nonce;
        public byte[] serverKey;
    }
}