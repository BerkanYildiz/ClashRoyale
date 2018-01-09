namespace ClashRoyale.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;

    public class NetworkToken
    {
        private readonly List<byte> Packet;

        public Socket Socket;
        public Device Device;
        public SocketAsyncEventArgs AsyncEvent;

        private int Fails;

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        public bool IsConnected
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
        /// Gets a value indicating whether this instance is failing.
        /// </summary>
        public bool IsFailing
        {
            get
            {
                return this.IsAborting || this.Fails >= 10;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is aborting.
        /// </summary>
        public bool IsAborting
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkToken"/> class.
        /// </summary>
        /// <param name="AsyncEvent">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <param name="Socket">The socket.</param>
        public NetworkToken(SocketAsyncEventArgs AsyncEvent, Socket Socket)
        {
            this.Packet                 = new List<byte>(Config.BufferSize);
            this.Socket                 = Socket;
            this.AsyncEvent             = AsyncEvent;
            this.AsyncEvent.UserToken   = this;
        }

        /// <summary>
        /// Sets the device.
        /// </summary>
        /// <param name="Device">The device.</param>
        public void SetDevice(Device Device)
        {
            this.Device                 = Device;
            this.Device.Token           = this;
        }

        /// <summary>
        /// Processes the data.
        /// </summary>
        public void AddData()
        {
            byte[] TempBuffer = new byte[this.AsyncEvent.BytesTransferred];
            Buffer.BlockCopy(this.AsyncEvent.Buffer, 0, TempBuffer, 0, this.AsyncEvent.BytesTransferred);
            this.Packet.AddRange(TempBuffer);
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        public void Process()
        {
            byte[] Buffer = this.Packet.ToArray();

            if (Buffer.Length >= 7 && Buffer.Length <= Config.BufferSize)
            {
                this.TcpProcess(Buffer);
            }
            else
            {
                NetworkTcp.Disconnect(this.AsyncEvent);
            }
        }

        /// <summary>
        /// Processes the buffer according to the TCP method.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        public void TcpProcess(byte[] Buffer)
        {
            short Type      = (short) (Buffer[1] | Buffer[0] << 8);
            int Length      = Buffer[4] | Buffer[3] << 8 | Buffer[2] << 16;
            short Version   = (short) (Buffer[6] | Buffer[5] << 8);

            if (Length < 0x800000)
            {
                if (Buffer.Get(7, Length, out byte[] Packet))
                {
                    this.Device.NetworkManager.ReceiveMessage(Type, Version, Packet);
                    this.Packet.RemoveRange(0, Length + 7);
                    
                    if (Buffer.Length - 7 - Length >= 7)
                    {
                        this.TcpProcess(Buffer.Get(Length + 7, Buffer.Length - 7 - Length));
                    }
                }
                else
                {
                    this.Fails++;
                }
            }
            else
            {
                NetworkTcp.Disconnect(this.AsyncEvent);
            }
        }
    }
}