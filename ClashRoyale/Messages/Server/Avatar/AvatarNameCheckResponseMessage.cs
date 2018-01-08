namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AvatarNameCheckResponseMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20300;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameCheckResponseMessage"/> class.
        /// </summary>
        public AvatarNameCheckResponseMessage()
        {
            // AvatarNameCheckResponseMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameCheckResponseMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AvatarNameCheckResponseMessage(ByteStream Stream) : base(Stream)
        {
            // AvatarNameCheckResponseMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteByte(0);
            this.Stream.WriteByte(0);
            this.Stream.WriteString(string.Empty);
        }
    }
}