namespace ClashRoyale.Messages.Server.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class GamecenterAccountBoundMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24211;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        private int ResultCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamecenterAccountBoundMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public GamecenterAccountBoundMessage(ByteStream Stream) : base(Stream)
        {
            // GamecenterAccountBoundMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ResultCode = this.Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(this.ResultCode);
        }
    }
}
