namespace ClashRoyale.Network
{
    using System.Collections.Concurrent;
    using System.Net.Sockets;

    public class NetworkPool : ConcurrentStack<SocketAsyncEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkPool"/> class.
        /// </summary>
        public NetworkPool()
        {
            for (int i = 0; i < Config.MaxPlayers; i++)
            {
                var AsyncEvent = new SocketAsyncEventArgs();

                AsyncEvent.SetBuffer(new byte[Config.BufferSize], 0, Config.BufferSize);
                AsyncEvent.DisconnectReuseSocket = true;

                this.Enqueue(AsyncEvent);
            }
        }

        /// <summary>
        /// Dequeues one of the available <see cref="SocketAsyncEventArgs"/>.
        /// </summary>
        public SocketAsyncEventArgs Dequeue()
        {
            if (this.TryPop(out SocketAsyncEventArgs AsyncEvent))
            {
                return AsyncEvent;
            }

            return null;
        }

        /// <summary>
        /// Enqueues the specified asynchronous event.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        public void Enqueue(SocketAsyncEventArgs AsyncEvent)
        {
            AsyncEvent.AcceptSocket     = null;
            AsyncEvent.RemoteEndPoint   = null;

            if (AsyncEvent.DisconnectReuseSocket)
            {
                this.Push(AsyncEvent);
            }
        }
    }
}