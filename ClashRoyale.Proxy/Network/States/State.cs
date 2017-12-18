namespace ClashRoyale.Proxy.Network.States
{
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class State
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        public State()
        {
            this.Packet = new List<byte>();
            this.Buffer = new byte[State.BufferSize];
        }

        public const int BufferSize     = 4096 * 1;
        public byte[] Buffer;
        public Socket Socket;
        public int Offset;

        public List<byte> Packet;
    }
}