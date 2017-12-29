namespace ClashRoyale.Proxy.Network
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Proxy.Logic;
    using ClashRoyale.Proxy.Logic.Collections;

    internal class Packet
    {
        internal Device Device;
        internal Socket Client;

        internal Destination Destination;

        internal short Identifier;
        internal int Length;
        internal short Version;

        internal byte[] Payload;
        internal byte[] EncryptedData;
        internal byte[] DecryptedData;

        internal string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        /// <param name="Destination">The destination.</param>
        /// <param name="Client">The client.</param>
        internal Packet(byte[] Buffer, Destination Destination, Socket Client)
        {
            this.Client             = Client;
            this.Device             = Devices.Get(Client.Handle);

            using (ByteStream Stream = new ByteStream(Buffer))
            {
                this.Destination    = Destination;

                this.Identifier     = Stream.ReadShort();
                this.Length         = Stream.ReadInt24();
                this.Version        = Stream.ReadShort();

                this.Payload        = Stream.ReadBytesWithoutLength(this.Length);
            }

            this.Name               = PacketType.GetName(this.Identifier);

            Logging.Info(this.GetType(), "Processing packet (" + this.Name + " | " + this.Identifier + ") " + Destination.ToString().Replace("_", " ").ToLower() + ", with version " + this.Version + ".");

            this.DecryptedData      = this.Device.EnDecrypt.Decrypt(this);
            // this.Device.Receive(this.Identifier, ref this.DecryptedData);
            this.EncryptedData      = this.Device.EnDecrypt.Encrypt(this);

            // Logging.Info(this.GetType(), BitConverter.ToString(this.RebuiltEncrypted));

            File.AppendAllText("Logs\\" + ((IPEndPoint) this.Client.RemoteEndPoint).Address + "\\TCP\\" + this.Name + "_" + this.Identifier + ".bin", BitConverter.ToString(this.RebuiltDecrypted) + Environment.NewLine);
        }

        /// <summary>
        /// Raw, re-encrypted packet (header included) 7 byte header + n byte payload Reverse()
        /// because of little endian byte order
        /// </summary>
        internal byte[] Rebuilt
        {
            get
            {
                byte[] Buffer;

                using (ByteStream Packet = new ByteStream(7 + this.Length))
                {
                    Packet.WriteShort(this.Identifier);
                    Packet.WriteInt24(this.Length);
                    Packet.WriteShort(this.Version);
                    Packet.AddRange(this.Payload);

                    Buffer = Packet.ToArray();
                }

                return Buffer;
            }
        }

        /// <summary>
        /// Raw, re-encrypted packet (header included) 7 byte header + n byte payload Reverse()
        /// because of little endian byte order
        /// </summary>
        internal byte[] RebuiltEncrypted
        {
            get
            {
                byte[] Buffer;

                using (ByteStream Packet = new ByteStream(7 + this.Length))
                {
                    Packet.WriteShort(this.Identifier);
                    Packet.WriteInt24(this.Length);
                    Packet.WriteShort(this.Version);
                    Packet.AddRange(this.EncryptedData);

                    Buffer = Packet.ToArray();
                }

                return Buffer;
            }
        }

        /// <summary>
        /// Raw, decrypted packet (header included) 7 byte header + n byte payload Reverse()
        /// because of little endian byte order
        /// </summary>
        internal byte[] RebuiltDecrypted
        {
            get
            {
                byte[] Buffer;

                using (ByteStream Packet = new ByteStream(7 + this.Length))
                {
                    Packet.WriteShort(this.Identifier);
                    Packet.WriteInt24(this.Length);
                    Packet.WriteShort(this.Version);
                    Packet.AddRange(this.DecryptedData);

                    Buffer = Packet.ToArray();
                }

                return Buffer;
            }
        }
    }
}