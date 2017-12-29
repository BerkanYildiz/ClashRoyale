namespace ClashRoyale.Server.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Network.Packets;

    internal static class TcpGateway
    {
        private static NetworkPool ReadPool;
        private static NetworkPool WritePool;

        private static Socket Listener;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TcpGateway"/> has been already initialized.
        /// </summary>
        private static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkGateway"/> class.
        /// </summary>
        internal static void Initialize()
        {
            if (TcpGateway.Initialized)
            {
                return;
            }

            ReadPool   = new NetworkPool();
            WritePool  = new NetworkPool();

            foreach (var AsyncEvent in TcpGateway.ReadPool)
            {
                AsyncEvent.Completed += TcpGateway.OnReceiveCompleted;
            }

            foreach (var AsyncEvent in TcpGateway.WritePool)
            {
                AsyncEvent.Completed += TcpGateway.OnSendCompleted;
            }

            Listener            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.NoDelay    = true;
            Listener.Blocking   = false;

            Initialized         = true;

            Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            Listener.Listen(150);

            Logging.Info(typeof(TcpGateway), "Listener has been bound to " + TcpGateway.Listener.LocalEndPoint + ".");

            SocketAsyncEventArgs AcceptEvent = new SocketAsyncEventArgs();
            AcceptEvent.Completed += OnAcceptCompleted;
            AcceptEvent.DisconnectReuseSocket = true;

            StartAccept(AcceptEvent);
        }

        /// <summary>
        /// Accepts a TCP Request.
        /// </summary>
        /// <param name="AcceptEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void StartAccept(SocketAsyncEventArgs AcceptEvent)
        {
            AcceptEvent.AcceptSocket = null;
            AcceptEvent.RemoteEndPoint = null;

            if (!Listener.AcceptAsync(AcceptEvent))
            {
                OnAcceptCompleted(null, AcceptEvent);
            }
        }

        /// <summary>
        /// Called when the client has been accepted.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                ProcessAccept(AsyncEvent);
            }
            else
            {
                Logging.Warning(typeof(TcpGateway), AsyncEvent.SocketError + ", SocketError != Success at OnAcceptCompleted(Sender, AsyncEvent).");

                StartAccept(AsyncEvent);
            }
        }

        /// <summary>
        /// Accept the new client and store it in memory.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            Logging.Info(typeof(TcpGateway), "Connection from " + ((IPEndPoint) AsyncEvent.AcceptSocket.RemoteEndPoint).Address + ".");

            if (AsyncEvent.AcceptSocket.Connected && (AsyncEvent.AcceptSocket.RemoteEndPoint.ToString().StartsWith("192.168.0.") || AsyncEvent.AcceptSocket.RemoteEndPoint.ToString().StartsWith("192.168.1.")))
            {
                SocketAsyncEventArgs ReadEvent = ReadPool.Dequeue();

                if (ReadEvent != null)
                {
                    NetworkToken Token  = new NetworkToken(ReadEvent, AsyncEvent.AcceptSocket);
                    Device Device       = new Device(Token);

                    if (!Token.Socket.ReceiveAsync(ReadEvent))
                    {
                        TcpGateway.ProcessReceive(ReadEvent);
                    }
                }
                else
                {
                    Logging.Warning(typeof(TcpGateway), "Server is full, new connections cannot be accepted.");
                }
            }

            StartAccept(AsyncEvent);
        }

        /// <summary>
        /// Receives data from the specified client.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred == 0)
            {
                Disconnect(AsyncEvent);
            }

            if (AsyncEvent.SocketError != SocketError.Success)
            {
                Disconnect(AsyncEvent);
            }

            NetworkToken Token = (NetworkToken) AsyncEvent.UserToken;

            if (Token.IsConnected)
            {
                Token.AddData();

                try
                {
                    if (Token.Socket.Available == 0)
                    {
                        Token.Process();
                    }

                    if (Token.Aborting == false)
                    {
                        if (!Token.Socket.ReceiveAsync(AsyncEvent))
                        {
                            TcpGateway.ProcessReceive(AsyncEvent);
                        }
                    }
                    else
                    {
                        // Logging.Warning(typeof(TcpGateway), "Token.Aborting == true at ProcessReceive(" + AsyncEvent.RemoteEndPoint + ").");
                    }
                }
                catch (Exception)
                {
                    Disconnect(AsyncEvent);
                }
            }
            else
            {
                // Logging.Warning(typeof(TcpGateway), "Token.IsConnected != true at ProcessReceive(AsyncEvent).");
            }
        }

        /// <summary>
        /// Called when [receive completed].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                TcpGateway.ProcessReceive(AsyncEvent);
            }
            else
            {
                Disconnect(AsyncEvent);
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        internal static void Send(Message Message)
        {
            if (Message == null)
            {
                throw new ArgumentNullException(nameof(Message), "Message == null at Send(Message).");
            }

            NetworkToken Token = Message.Device.Network;

            if (Token.IsConnected)
            {
                SocketAsyncEventArgs WriteEvent = WritePool.Dequeue();

                if (WriteEvent == null)
                {
                    WriteEvent = new SocketAsyncEventArgs
                    {
                        DisconnectReuseSocket = false
                    };
                }

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket     = Token.Socket;
                WriteEvent.RemoteEndPoint   = Token.Socket.RemoteEndPoint;
                WriteEvent.UserToken        = Token;

                Logging.Info(typeof(TcpGateway), "Sending " + Message.GetType().Name + ".");

                if (!Token.Socket.SendAsync(WriteEvent))
                {
                    ProcessSend(Message, WriteEvent);
                }
            }
            else
            {
                Disconnect(Message.Device.Network.AsyncEvent);
            }
        }

        /// <summary>
        /// Processes to send the specified message using the specified SocketAsyncEventArgs.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void ProcessSend(Message Message, SocketAsyncEventArgs AsyncEvent)
        {
            NetworkToken Token = (NetworkToken) AsyncEvent.UserToken;

            if (AsyncEvent.SocketError == SocketError.Success)
            {
                Message.Offset += AsyncEvent.BytesTransferred;

                if (Message.Length + 7 > Message.Offset)
                {
                    if (Message.Device.Network.IsConnected)
                    {
                        AsyncEvent.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                        if (!Token.Socket.SendAsync(AsyncEvent))
                        {
                            ProcessSend(Message, AsyncEvent);
                        }

                        return;
                    }

                    Disconnect(Token.AsyncEvent);
                }
                else
                {
                    Logging.Warning(typeof(TcpGateway), "Shouldn't be called."); // TODO : Verifiy if it's correct.
                }
            }
            else
            {
                Disconnect(Token.AsyncEvent);
            }

            OnSendCompleted(null, AsyncEvent);
        }

        /// <summary>
        /// Called when [send completed].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            WritePool.Enqueue(AsyncEvent);
        }

        /// <summary>
        /// Closes the specified client's socket.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        internal static void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            NetworkToken Token = AsyncEvent.UserToken as NetworkToken;

            if (Token.Aborting)
            {
                return;
            }

            Token.Aborting = true;

            if (Token.Device != null)
            {
                Token.Device.State = State.Disconnected;

                try
                {
                    Token.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // Already Closed.
                }

                Token.Socket.Close();
            }

            ReadPool.Enqueue(AsyncEvent);
        }
    }
}
