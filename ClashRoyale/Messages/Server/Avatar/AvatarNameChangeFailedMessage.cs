namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AvatarNameChangeFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20205;
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

        public int ErrorCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameChangeFailedMessage"/> class.
        /// </summary>
        public AvatarNameChangeFailedMessage()
        {
            // AvatarNameChangeFailedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameChangeFailedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public AvatarNameChangeFailedMessage(ByteStream Stream) : base(Stream)
        {
            // AvatarNameChangeFailedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameChangeFailedMessage"/> class.
        /// </summary>
        /// <param name="ErrorCode">The error code.</param>
        public AvatarNameChangeFailedMessage(int ErrorCode)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ErrorCode = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.ErrorCode);
        }
    }
}