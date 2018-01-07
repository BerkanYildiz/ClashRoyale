namespace ClashRoyale.Messages.Client.Socials.Bind
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class BindFacebookAccount : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14201;
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

        public string FbIdentifier;
        public string FbToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindFacebookAccount"/> class.
        /// </summary>
        public BindFacebookAccount()
        {
            // BindFacebookAccount.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindFacebookAccount"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public BindFacebookAccount(ByteStream Stream) : base(Stream)
        {
            // BindFacebookAccount.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Force        = this.Stream.ReadBoolean();
            this.FbIdentifier = this.Stream.ReadString();
            this.FbToken      = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(this.Force);
            this.Stream.WriteString(this.FbIdentifier);
            this.Stream.WriteString(this.FbToken);
        }
    }
}