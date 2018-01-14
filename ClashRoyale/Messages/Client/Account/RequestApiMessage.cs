namespace ClashRoyale.Messages.Client.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Messages;

    public class RequestApiMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 15080;
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
        /// Initializes a new instance of the <see cref="RequestApiMessage"/> class.
        /// </summary>
        public RequestApiMessage()
        {
            // RequestApiMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestApiMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public RequestApiMessage(ByteStream Stream) : base(Stream)
        {
            // RequestApiMessage.
        }
    }
}