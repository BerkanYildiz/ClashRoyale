namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class MatchmakeInfoMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24107;
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

        public int EstimatedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchmakeInfoMessage"/> class.
        /// </summary>
        public MatchmakeInfoMessage()
        {
            // MatchmakeInfoMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchmakeInfoMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="EstimedTime">The estimed time.</param>
        public MatchmakeInfoMessage(ByteStream Stream) : base(Stream)
        {
            // MatchmakeInfoMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.EstimatedTime = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.EstimatedTime);
        }
    }
}