namespace ClashRoyale.Client.Packets
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Client.Logic;

    internal class Command
    {
        internal int Identifier;

        internal int SubTick1;
        internal int SubTick2;

        internal int SubHighID;
        internal int SubLowID;

        internal Reader Reader;
        internal Device Device;

        internal List<byte> Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal Command(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        /// <param name="Device">The device.</param>
        /// <param name="Identifier">The identifier.</param>
        internal Command(Reader Reader, Device Device, int Identifier)
        {
            this.Identifier = Identifier;
            this.Device = Device;
            this.Reader = Reader;

            this.Data = new List<byte>();
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddVInt(this.Identifier);
                Packet.AddRange(this.Data);

                return Packet.ToArray();
            }
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal virtual void Decode()
        {
            // Decode.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode()
        {
            // Encode.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal virtual void Process()
        {
            // Process.
        }

        /// <summary>
        /// Reads the header.
        /// </summary>
        internal void ReadHeader()
        {
            this.SubTick1 = this.Reader.ReadVInt();
            this.SubTick2 = this.Reader.ReadVInt();
            this.SubHighID = this.Reader.ReadVInt();
            this.SubLowID = this.Reader.ReadVInt();
        }

        /// <summary>
        /// Debugs this instance.
        /// </summary>
        internal void Debug()
        {
            Console.WriteLine(ConsolePad.Padding(this.GetType().Name, 15) + " : " + BitConverter.ToString(this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position))));
        }
    }
}