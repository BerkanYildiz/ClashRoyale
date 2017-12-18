namespace ClashRoyale.Client.Core.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    using ClashRoyale.Client.Logic;

    internal class Token
    {
        internal Device Device;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;

        internal byte[] Buffer;

        internal bool Aborting;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="Args">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <param name="Device">The device.</param>
        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device         = Device;
            this.Device.Token   = this;

            this.Args           = Args;
            this.Args.UserToken = this;

            this.Buffer         = new byte[Constants.ReceiveBuffer];
            this.Packet         = new List<byte>(Constants.ReceiveBuffer);
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        internal void SetData()
        {
            byte[] Data = new byte[this.Args.BytesTransferred];
            Array.Copy(this.Args.Buffer, 0, Data, 0, this.Args.BytesTransferred);
            this.Packet.AddRange(Data);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal void Process()
        {
            byte[] Data = this.Packet.ToArray();
            this.Device.Process(Data);
        }
    }
}