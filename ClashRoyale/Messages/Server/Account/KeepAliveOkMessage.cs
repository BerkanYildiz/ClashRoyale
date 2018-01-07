namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class KeepAliveOkMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24135;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveOkMessage"/> class.
        /// </summary>
        public KeepAliveOkMessage()
        {
            // KeepAliveOkMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveOkMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public KeepAliveOkMessage(ByteStream Stream) : base(Stream)
        {
            // KeepAliveOkMessage.
        }
    }
}