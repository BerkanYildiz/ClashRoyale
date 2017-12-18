namespace ClashRoyale.Client.Core.Network
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Packets;
    using ClashRoyale.Client.Packets.Crypto;

    internal class Gateway
    {
        private Device Device;

        private int Offset;

        internal static int TotalMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class.
        /// </summary>
        internal Gateway()
        {
            // Gateway.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal Gateway(Device Device) : this()
        {
            this.Device = Device;
            this.Device.Token.Args.Completed += this.OnReceiveCompleted;
            this.Device.Token.Args.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
        }

        /// <summary>
        /// Receives this instance.
        /// </summary>
        internal void Receive()
        {
            if (!this.Device.Socket.ReceiveAsync(this.Device.Token.Args))
            {
                this.ProcessReceive(this.Device.Token.Args);
            }
        }

        /// <summary>
        /// Receives data from the specified client.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                Token Token = AsyncEvent.UserToken as Token;

                if (!Token.Aborting)
                {
                    Token.SetData();

                    try
                    {
                        if (Token.Device.Socket.Available == 0)
                        {
                            Token.Process();

                            if (!Token.Aborting)
                            {
                                if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                                {
                                    this.ProcessReceive(AsyncEvent);
                                }
                            }
                        }
                        else
                        {
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("[*] Warning : We got disconnected by the server !");
                    }
                }
            }
            else
            {
                Debug.WriteLine("[*] Warning : We got disconnected by the server !");
            }
        }

        /// <summary>
        /// Called when [receive completed].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                this.ProcessReceive(AsyncEvent);
            }
            else
            {
                Debug.WriteLine("[*] Warning : We got disconnected by the server !");
            }
        }

        /// <summary>
        /// Sends this instance.
        /// </summary>
        internal void Send(Message Message)
        {
            if (Message != null)
            {
                Message.Encode();

                Debug.WriteLine("[*] We are sending the following message : ID " + Message.Identifier + ", Length " + Message.Length + ", Version " + Message.Version + ".");

                byte[] Packet = Message.Data.ToArray();

                Device Device = Message.Device;

                if (Device.SendEncrypter != null)
                {
                    Packet = Device.SendEncrypter.Encrypt(Packet);
                }
                else
                {
                    if (Device.PepperInit.State > 0)
                    {
                        if (Device.PepperInit.State == 2)
                        {
                            Packet = PepperCrypto.SendPepperLogin(ref Device.PepperInit, Packet);
                        }
                        else
                        {
                            Console.WriteLine(this.Device.State + ": " + Message.Identifier);
                            Packet = null;
                        }
                    }
                    else
                    {
                        Packet = PepperCrypto.SendPepperAuthentification(ref Device.PepperInit, Packet);
                    }
                }

                if (Packet != null)
                {
                    Message.Length = (uint) Packet.Length;

                    Message.Data.Clear();
                    Message.Data.AddRange(Packet);

                    byte[] Buffer = Message.ToBytes;

                    if (this.Device.Connected)
                    {
                        try
                        {
                            this.Device.Socket.BeginSend(Buffer, 0, Buffer.Length, 0, this.SendCallback, Message);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        Debug.WriteLine("[*] Warning : Aborting the send process because we are disconnected !");
                    }
                }
            }
            else
            {
                Debug.WriteLine("[*] Warning : Message was null at send !");
            }
        }

        private void SendCallback(IAsyncResult AsyncResult)
        {
            Message Message = (Message) AsyncResult.AsyncState;

            if (Message != null)
            {
                if (!this.Device.Token.Aborting)
                {
                    ++Gateway.TotalMessage;

                    int BytesSent = this.Device.Socket.EndSend(AsyncResult);

                    if (BytesSent < Message.Length + 7)
                    {
                        Debug.WriteLine("[*] Warning : We still have bytes to send !");
                    }
                }
            }
            else
            {
                Debug.WriteLine("[*] Warning : Message was null at send callback !");
            }
        }
    }
}