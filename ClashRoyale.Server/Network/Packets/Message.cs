namespace ClashRoyale.Server.Network.Packets
{
    using System;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class Message
    {
        internal short Version;
        internal int Offset;

        /// <summary>
        /// The device, technically called as 'client'.
        /// </summary>
        internal Device Device;
        
        /// <summary>
        /// The message stream, used to.. read or write the message.
        /// </summary>
        internal ByteStream Stream;

        /// <summary>
        /// Gets a value indicating whether this message is a server to client message.
        /// </summary>
        internal bool IsServerToClientMessage
        {
            get
            {
                return this.Type >= 20000;
            }
        }

        /// <summary>
        /// Gets the encoding length.
        /// </summary>
        internal int Length
        {
            get
            {
                return this.Stream.Length;
            }
        }

        /// <summary>
        /// The type of this message.
        /// </summary>
        internal virtual short Type
        {
            get
            {
                throw new Exception(this.GetType() + ", type must be overridden.");
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal virtual Node ServiceNode
        {
            get
            {
                throw new Exception(this.GetType() + ", service node type must be overridden.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        internal Message()
        {
            // Message.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal Message(Device Device) : this()
        {
            this.Device = Device;
            this.Stream = new ByteStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Stream">The stream.</param>
        internal Message(Device Device, ByteStream Stream) : this()
        {
            this.Device = Device;
            this.Stream = Stream;
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
        /// Gets or sets the packet data, in/from a byte array.
        /// </summary>
        /// <returns>The packet data, in a byte array, header included.</returns>
        internal byte[] ToBytes
        {
            get
            {
                byte[] Buffer;

                using (ByteStream Packet = new ByteStream(7 + this.Length))
                {
                    Packet.WriteShort(this.Type);
                    Packet.WriteInt24(this.Length);
                    Packet.WriteShort(this.Version);
                    Packet.AddRange(this.Stream.ToArray());

                    Buffer = Packet.ToArray();
                }

                return Buffer;
            }
        }
    }
}