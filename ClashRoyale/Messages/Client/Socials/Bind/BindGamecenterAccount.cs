namespace ClashRoyale.Messages.Client.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class BindGamecenterAccount : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14212;
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

        public string GamecenterId;
        public string Certificate;
        public string AppBundle;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindGamecenterAccount"/> class.
        /// </summary>
        public BindGamecenterAccount()
        {
            // BindGamecenterAccount.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindGamecenterAccount"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public BindGamecenterAccount(ByteStream Stream) : base(Stream)
        {
            // BindGamecenterAccount.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Force          = this.Stream.ReadBoolean();

            this.GamecenterId   = this.Stream.ReadString();
            this.Certificate    = this.Stream.ReadString();
            this.AppBundle      = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteBoolean(this.Force);

            this.Stream.WriteString(this.GamecenterId);
            this.Stream.WriteString(this.Certificate);
            this.Stream.WriteString(this.AppBundle);
        }
    }
}