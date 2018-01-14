namespace ClashRoyale.Messages.Client.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class BindGoogleAccountMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14997;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Friend;
            }
        }

        public bool Force;

        public string GoogleId;
        public string GoogleToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindGoogleAccountMessage"/> class.
        /// </summary>
        public BindGoogleAccountMessage()
        {
            // BindGoogleAccountMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindGoogleAccountMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public BindGoogleAccountMessage(ByteStream Stream) : base(Stream)
        {
            // BindGoogleAccountMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Force          = this.Stream.ReadBoolean();
            this.GoogleId       = this.Stream.ReadString();
            this.GoogleToken    = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(this.Force);
            this.Stream.WriteString(this.GoogleId);
            this.Stream.WriteString(this.GoogleToken);
        }
    }
}