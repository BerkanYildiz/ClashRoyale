namespace ClashRoyale.Client.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    using ClashRoyale.Extensions;

    internal class NetworkToken
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NetworkToken"/> is aborting.
        /// </summary>
        internal bool Aborting
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        internal SocketAsyncEventArgs Args
        {
            get;
            set;
        }

        private readonly NetworkGateway Gateway;
        private readonly List<byte> Packet;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkToken"/> class.
        /// </summary>
        internal NetworkToken()
        {
            this.Packet = new List<byte>(Config.BufferSize);
            this.Args   = new SocketAsyncEventArgs();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkToken"/> class.
        /// </summary>
        /// <param name="Gateway">The gateway.</param>
        internal NetworkToken(NetworkGateway Gateway) : this()
        {
            this.Gateway = Gateway;
        }

        /// <summary>
        /// Processes the data.
        /// </summary>
        internal void AddData()
        {
            if (Args.BytesTransferred > 0)
            {
                byte[] TempBuffer = new byte[Args.BytesTransferred];
                Buffer.BlockCopy(Args.Buffer, 0, TempBuffer, 0, Args.BytesTransferred);
                this.Packet.AddRange(TempBuffer);
            }
            else
            {
                this.Aborting = true;
            }
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        internal void Process()
        {
            byte[] Buffer = this.Packet.ToArray();

            if (Buffer.Length >= 7)
            {
                this.TcpProcess(Buffer);
            }
        }

        /// <summary>
        /// Processes the buffer according to the TCP method.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        internal void TcpProcess(byte[] Buffer)
        {
            short Type      = (short) (Buffer[1] | Buffer[0] << 8);
            int Length      = Buffer[4] | Buffer[3] << 8 | Buffer[2] << 16;
            short Version   = (short) (Buffer[6] | Buffer[5] << 8);

            if (Length < 0x800000)
            {
                if (Buffer.Get(7, Length, out byte[] Packet))
                {
                    this.Gateway.Manager.ReceiveMessage(Type, Version, Packet);
                    this.Packet.RemoveRange(0, Length + 7);
                    
                    if (Buffer.Length - 7 - Length >= 7)
                    {
                        this.TcpProcess(Buffer.Get(Length + 7, Buffer.Length - 7 - Length));
                    }
                }
            }
            else
            {
                this.Aborting = true;
            }
        }
    }
}