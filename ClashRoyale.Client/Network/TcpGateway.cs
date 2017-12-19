namespace ClashRoyale.Client.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets;
    using ClashRoyale.Enums;

    internal static class TcpGateway
    {
        private static NetworkToken ReadEvent;
        private static NetworkToken WriteEvent;

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

            TcpGateway.ReadEvent    = new NetworkToken();
            TcpGateway.WriteEvent   = new NetworkToken();

            foreach (var AsyncEvent in TcpGateway.ReadPool)
            {
                AsyncEvent.Completed += TcpGateway.OnReceiveCompleted;
            }

            foreach (var AsyncEvent in TcpGateway.WritePool)
            {
                AsyncEvent.Completed += TcpGateway.OnSendCompleted;
            }

            TcpGateway.Listener            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpGateway.Listener.NoDelay    = true;
            TcpGateway.Listener.Blocking   = false;

            TcpGateway.Initialized         = true;

            Logging.Info(typeof(TcpGateway), "Listener has been bound to " + TcpGateway.Listener.LocalEndPoint + ".");
        }

        /// <summary>
        /// Connects using a TCP Request.
        /// </summary>
        /// <param name="AcceptEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void StartConnect(SocketAsyncEventArgs ConnectEvent)
        {
            ConnectEvent.AcceptSocket   = null;
            ConnectEvent.RemoteEndPoint = new DnsEndPoint("game.clashroyaleapp.com", 9339);

            if (!TcpGateway.Listener.ConnectAsync(ConnectEvent))
            {
                TcpGateway.OnConnectCompleted(null, ConnectEvent);
            }
        }

        /// <summary>
        /// Called when the client has been connected.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void OnConnectCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                Logging.Info(typeof(TcpGateway), "Gateway is connected.");
            }
            else
            {
                Logging.Warning(typeof(TcpGateway), AsyncEvent.SocketError + ", SocketError != Success at OnConnectCompleted(Sender, AsyncEvent).");

                TcpGateway.StartConnect(AsyncEvent);
            }
        }

        /// <summary>
        /// Connect the new client and store it in memory.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void ProcessConnect(SocketAsyncEventArgs AsyncEvent)
        {
            Logging.Info(typeof(TcpGateway), "Connection from " + ((IPEndPoint) AsyncEvent.AcceptSocket.RemoteEndPoint).Address + ".");

            if (AsyncEvent.AcceptSocket.Connected)
            {
                SocketAsyncEventArgs ReadEvent = TcpGateway.ReadPool.Dequeue();

                if (ReadEvent == null)
                {
                    ReadEvent = new SocketAsyncEventArgs();
                    ReadEvent.Completed += TcpGateway.OnReceiveCompleted;
                }

                NetworkToken Token  = new NetworkToken(ReadEvent, AsyncEvent.AcceptSocket);
                Device Device       = new Device(Token);

                if (!Token.Socket.ReceiveAsync(ReadEvent))
                {
                    TcpGateway.ProcessReceive(ReadEvent);
                }
            }
        }

        /// <summary>
        /// Receives data from the specified client.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred == 0)
            {
                TcpGateway.Disconnect(AsyncEvent);
            }

            if (AsyncEvent.SocketError != SocketError.Success)
            {
                TcpGateway.Disconnect(AsyncEvent);
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
                    TcpGateway.Disconnect(AsyncEvent);
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
                TcpGateway.Disconnect(AsyncEvent);
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
                SocketAsyncEventArgs WriteEvent = TcpGateway.WritePool.Dequeue();

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
                    TcpGateway.ProcessSend(Message, WriteEvent);
                }
            }
            else
            {
                TcpGateway.Disconnect(Message.Device.Network.AsyncEvent);
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
                            TcpGateway.ProcessSend(Message, AsyncEvent);
                        }

                        return;
                    }

                    TcpGateway.Disconnect(Token.AsyncEvent);
                }
                else
                {
                    Logging.Warning(typeof(TcpGateway), "Shouldn't be called."); // TODO : Verifiy if it's correct.
                }
            }
            else
            {
                TcpGateway.Disconnect(Token.AsyncEvent);
            }

            TcpGateway.OnSendCompleted(null, AsyncEvent);
        }

        /// <summary>
        /// Called when [send completed].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private static void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            TcpGateway.WritePool.Enqueue(AsyncEvent);
        }

        /// <summary>
        /// Closes the specified client's socket.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        internal static void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            NetworkToken Token = AsyncEvent.UserToken as NetworkToken;

            if (!Token.Aborting)
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

            TcpGateway.ReadPool.Enqueue(AsyncEvent);
        }
    }
}
