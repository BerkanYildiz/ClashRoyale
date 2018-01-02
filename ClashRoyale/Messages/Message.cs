namespace ClashRoyale.Messages
{
    using System;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;

    public class Message
    {
        /// <summary>
        /// The type of this <see cref="Message"/>.
        /// </summary>
        public virtual short Type
        {
            get
            {
                throw new Exception(this.GetType() + ", type must be overridden.");
            }
        }

        /// <summary>
        /// The service node of this <see cref="Message"/>.
        /// </summary>
        public virtual Node ServiceNode
        {
            get
            {
                throw new Exception(this.GetType() + ", service node type must be overridden.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Message"/> is from the server to the client.
        /// </summary>
        public bool IsServerToClientMessage
        {
            get
            {
                return this.Type >= 20000;
            }
        }

        /// <summary>
        /// Gets the length of this <see cref="Message"/>
        /// </summary>
        public int Length
        {
            get
            {
                return this.Stream.Length;
            }
        }
        
        /// <summary>
        /// Gets the offset of this <see cref="Message"/>.
        /// </summary>
        public int Offset
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the version of this <see cref="Message"/>.
        /// </summary>
        public short Version
        {
            get;
            set;
        }

        /// <summary>
        /// The device, technically called as 'client'.
        /// </summary>
        public Device Device
        {
            get;
            set;
        }
        
        /// <summary>
        /// The message stream, used to.. read or write the message.
        /// </summary>
        public ByteStream Stream
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            // Message.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Message(Device Device) : this()
        {
            this.Device = Device;
            this.Stream = new ByteStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Stream">The stream.</param>
        public Message(Device Device, ByteStream Stream) : this()
        {
            this.Device = Device;
            this.Stream = Stream;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public virtual void Decode()
        {
            // Decode.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public virtual void Encode()
        {
            // Encode.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public virtual void Process()
        {
            // Process.
        }

        /// <summary>
        /// Gets the packet data, in/from a byte array.
        /// </summary>
        /// <returns>The packet data, in a byte array, header included.</returns>
        public byte[] ToBytes
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

        /// <summary>
        /// Gets the packet data, in/from an hexa string.
        /// </summary>
        public string ToHexa
        {
            get
            {
                return BitConverter.ToString(this.Stream.ToArray(this.Stream.Offset, this.Stream.BytesLeft));
            }
        }
    }
}