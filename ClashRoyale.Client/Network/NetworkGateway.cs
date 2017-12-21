namespace ClashRoyale.Client.Network
{
    using System;
    using System.Net.Sockets;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets;

    internal class NetworkGateway
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="NetworkGateway"/> is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (this.Socket.Connected)
                {
                    try
                    {
                        if (!this.Socket.Poll(1000, SelectMode.SelectRead) || this.Socket.Available != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the <see cref="AddressFamily"/> used for this instance.
        /// </summary>
        internal AddressFamily Interface
        {
            get
            {
                return this.Socket.AddressFamily;
            }
        }

        /// <summary>
        /// Gets the <see cref="Bot"/> aka bot, used to manage and handle packets.
        /// </summary>
        private Bot Bot
        {
            get
            {
                if (this.Manager != null)
                {
                    return this.Manager.Bot;
                }

                return null;
            }
        }

        internal NetworkManager Manager;

        private NetworkToken Token;
        private Socket Socket;

        private bool Aborting;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkGateway"/> class.
        /// </summary>
        internal NetworkGateway()
        {
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Token  = new NetworkToken(this);
            this.Token.Args.Completed += this.OnReceiveCompleted;
            this.Token.Args.SetBuffer(new byte[Config.BufferSize], 0, Config.BufferSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkGateway"/> class.
        /// </summary>
        /// <param name="Device">The bot.</param>
        internal NetworkGateway(NetworkManager Manager) : this()
        {
            this.Manager = Manager;
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal void Connect(string Host)
        {
            this.Socket.Connect(Host, 9339);

            if (this.IsConnected)
            {
                this.Receive();
            }
        }

        /// <summary>
        /// Receives this instance.
        /// </summary>
        private void Receive()
        {
            if (!this.Socket.ReceiveAsync(this.Token.Args))
            {
                this.ProcessReceive();
            }
        }

        /// <summary>
        /// Receives data from the specified client.
        /// </summary>
        private void ProcessReceive()
        {
            if (this.Token.Args.BytesTransferred > 0 && this.Token.Args.SocketError == SocketError.Success)
            {
                if (!this.Token.Aborting)
                {
                    Token.AddData();

                    try
                    {
                        if (this.Socket.Available == 0)
                        {
                            Token.Process();

                            if (!Token.Aborting)
                            {
                                if (!this.Socket.ReceiveAsync(this.Token.Args))
                                {
                                    this.ProcessReceive();
                                }
                            }
                        }
                        else
                        {
                            if (!this.Socket.ReceiveAsync(this.Token.Args))
                            {
                                this.ProcessReceive();
                            }
                        }
                    }
                    catch (Exception Exception)
                    {
                        Logging.Error(this.GetType(), "[" + this.Bot.BotId + "] " + Exception.GetType().Name + " at ProcessReceive() !");
                    }
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] Disconnected at ProcessReceive() !");
            }
        }

        /// <summary>
        /// Called when [receive completed].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                this.ProcessReceive();
            }
            else
            {
                this.Aborting = true;
            }
        }

        /// <summary>
        /// Sends this instance.
        /// </summary>
        internal void Send(Message Message)
        {
            if (this.IsConnected && this.Aborting == false)
            {
                if (Message != null)
                {
                    byte[] Buffer = Message.ToBytes;

                    if (Buffer != null)
                    {
                        this.Socket.BeginSend(Buffer, 0, Buffer.Length, 0, this.SendCallback, Message);
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "[" + this.Bot.BotId + "] Buffer == null at Send(Message) !");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "[" + this.Bot.BotId + "] Message == null at Send(Message) !");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] IsConnected == false at Send(Message) !");
            }
        }

        private void SendCallback(IAsyncResult AsyncResult)
        {
            if (AsyncResult.AsyncState is Message Message)
            {
                if (!this.Token.Aborting)
                {
                    int BytesSent = this.Socket.EndSend(AsyncResult);

                    if (BytesSent < Message.Length + 7)
                    {
                        Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] BytesSent < (Message.Length + 7) at SendCallback(AsyncResult) !");
                    }
                }
            }
            else
            {
                Logging.Error(this.GetType(), "[" + this.Bot.BotId + "] Message == null at SendCallback(AsyncResult) !");
            }
        }
    }
}