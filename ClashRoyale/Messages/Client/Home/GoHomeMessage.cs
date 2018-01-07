namespace ClashRoyale.Messages.Client.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class GoHomeMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14560;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        public int Unknown1;
        public int Unknown2;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoHomeMessage"/> class.
        /// </summary>
        public GoHomeMessage()
        {
            // GoHomeMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoHomeMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public GoHomeMessage(ByteStream Stream) : base(Stream)
        {
            // GoHomeMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Unknown1 = this.Stream.ReadInt();
            this.Unknown2 = this.Stream.ReadVInt();
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.Unknown1);
            this.Stream.WriteVInt(this.Unknown2);
        }
    }
}