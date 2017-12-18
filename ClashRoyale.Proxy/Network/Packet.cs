namespace ClashRoyale.Proxy.Network
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

            this.DecryptedData      = this.Payload;
            this.EncryptedData      = this.Payload;

            // this.DecryptedData      = this.Device.EnDecrypt.Decrypt(this);
            // this.Device.Receive(this.Identifier, ref this.DecryptedData);
            // this.EncryptedData      = this.Device.EnDecrypt.Encrypt(this);

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
                List<byte> Packet = new List<byte>();

                Packet.AddRange(BitConverter.GetBytes(this.Identifier).Reverse().Skip(2));
                Packet.AddRange(BitConverter.GetBytes(this.Payload.Length).Reverse().Skip(1));
                Packet.AddRange(BitConverter.GetBytes(this.Version).Reverse().Skip(2));
                Packet.AddRange(this.Payload);

                return Packet.ToArray();
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
                List<byte> Packet = new List<byte>();

                Packet.AddRange(BitConverter.GetBytes(this.Identifier).Reverse().Skip(2));
                Packet.AddRange(BitConverter.GetBytes(this.EncryptedData.Length).Reverse().Skip(1));
                Packet.AddRange(BitConverter.GetBytes(this.Version).Reverse().Skip(2));
                Packet.AddRange(this.EncryptedData);

                return Packet.ToArray();
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
                List<byte> Packet = new List<byte>();

                Packet.AddRange(BitConverter.GetBytes(this.Identifier).Reverse().Skip(2));
                Packet.AddRange(BitConverter.GetBytes(this.DecryptedData.Length).Reverse().Skip(1));
                Packet.AddRange(BitConverter.GetBytes(this.Version).Reverse().Skip(2));
                Packet.AddRange(this.DecryptedData);

                return Packet.ToArray();
            }
        }
    }
}