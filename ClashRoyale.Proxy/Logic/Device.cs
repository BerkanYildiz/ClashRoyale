namespace ClashRoyale.Proxy.Logic
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    using ClashRoyale.Extensions;
    using ClashRoyale.Proxy.Network;
    using ClashRoyale.Proxy.Packets;

    internal class Device
    {
        internal Processor Processor;
        internal EnDecrypt EnDecrypt;

        internal Socket Client;
        internal Socket Server;

        internal IPEndPoint UdpClientEndPoint;
        internal IPEndPoint UdpServerEndPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="Socket">The socket.</param>
        internal Device(Socket Socket)
        {
            this.Client     = Socket;
            this.Server     = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.Server.Connect("game.clashroyaleapp.com", 9339);

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs\\" + ((IPEndPoint) this.Client.RemoteEndPoint).Address + "\\TCP");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs\\" + ((IPEndPoint) this.Client.RemoteEndPoint).Address + "\\UDP");

            this.EnDecrypt  = new EnDecrypt();
            this.Processor  = new Processor(this.Client, this.Server);
        }

        /// <summary>
        /// Processes the packet.
        /// </summary>
        internal void Process(int Type, ref byte[] Packet)
        {
            using (ByteStream Reader = new ByteStream(Packet))
            {
                switch (Type)
                {
                    case 10100:
                    {
                        Packet[4 + 4 - 1] = 15;
                        Packet[4 + 4 + 4 - 1] = 0x03;
                        Packet[4 + 4 + 4 + 4 - 1] = 0x00;
                        Packet[4 + 4 + 4 + 4 + 4 - 2] = 0x03;
                        Packet[4 + 4 + 4 + 4 + 4 - 1] = 0x3E;
                        Packet[4 + 4 + 4 + 4 + 4 + (4 + 0x28) + 4 - 1] = 0x03;

                        Logging.Info(this.GetType(), BitConverter.ToString(Packet));
                        break;
                    }

                    case 10101:
                    {
                        if (Packet[4 + 4] == 0xFF)
                        {
                            Packet[4 + 4 + 4 + 4 - 1] = 0x03;
                            Packet[4 + 4 + 4 + 4 + 4 - 1] = 0x00;
                            Packet[4 + 4 + 4 + 4 + 4 + 4 - 1] = 0x03;
                            Packet[4 + 4 + 4 + 4 + 4 + 4 - 2] = 0x3E;

                            // Packet = Packet.Skip(4 + 4 + 4).ToArray();
                        }
                        else
                        {
                            Packet[4 + 4 + (4 + 0x28) + 4 - 1] = 0x03;
                            Packet[4 + 4 + (4 + 0x28) + 4 + 4 - 1] = 0x00;
                            Packet[4 + 4 + (4 + 0x28) + 4 + 4 + 4 - 1] = 0x03;
                            Packet[4 + 4 + (4 + 0x28) + 4 + 4 + 4 - 2] = 0x3E;

                            // Packet = Packet.Skip(4 + 4 + (4 + 40)).ToArray();
                        }

                        /* 
                        Logging.Info(this.GetType(), BitConverter.ToString(Packet));

                        byte[] High  = new byte[] { 0x00, 0x00, 0x00, 0x02};
                        byte[] Low   = new byte[] { 0x00, 0x00, 0x37, 0xEE};
                        byte[] Token = new byte[]
                        {
                            0x00, 0x00, 0x00, 0x28,
                            0x68, 0x77, 0x38, 0x72, 0x37, 0x32, 0x78, 0x66, 0x6B, 0x6D, 0x6E, 0x67, 0x65, 0x72, 0x39, 0x6A, 0x72, 0x77, 0x6E, 0x74,
                            0x77, 0x6B, 0x77, 0x68, 0x61, 0x33, 0x6B, 0x73, 0x61, 0x73, 0x32, 0x79, 0x77, 0x72, 0x62, 0x63, 0x61, 0x66, 0x77, 0x6A
                        };

                        Packet = High.Concat(Low.Concat(Token.Concat(Packet))).ToArray(); */

                        Logging.Info(this.GetType(), BitConverter.ToString(Packet));

                        break;
                    }
                }
            }
        }
    }
}