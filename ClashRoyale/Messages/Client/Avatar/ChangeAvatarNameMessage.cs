namespace ClashRoyale.Messages.Client.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class ChangeAvatarNameMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 19863;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        public string Username;
        public int ChangeState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameMessage"/> class.
        /// </summary>
        public ChangeAvatarNameMessage()
        {
            // ChangeAvatarNameMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ChangeAvatarNameMessage(ByteStream Stream) : base(Stream)
        {
            // ChangeAvatarNameMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Username = this.Stream.ReadString();
            this.ChangeState = this.Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Username);
            this.Stream.WriteVInt(this.ChangeState);
        }
    }
}