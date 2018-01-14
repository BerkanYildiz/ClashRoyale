namespace ClashRoyale.Messages.Client.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Messages;

    public class KeepAliveMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 19911;
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
        /// Initializes a new instance of the <see cref="KeepAliveMessage"/> class.
        /// </summary>
        public KeepAliveMessage()
        {
            // KeepAliveMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public KeepAliveMessage(ByteStream Stream) : base(Stream)
        {
            // KeepAliveMessage.
        }
    }
}