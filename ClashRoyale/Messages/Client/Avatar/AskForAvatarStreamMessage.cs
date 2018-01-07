namespace ClashRoyale.Messages.Client.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForAvatarStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 17101;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarStreamMessage"/> class.
        /// </summary>
        public AskForAvatarStreamMessage()
        {
            // AskForAvatarStreamMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarStreamMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForAvatarStreamMessage(ByteStream Stream) : base(Stream)
        {
            // AskForAvatarStreamMessage.
        }
    }
}