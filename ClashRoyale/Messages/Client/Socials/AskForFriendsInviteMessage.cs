namespace ClashRoyale.Messages.Client.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForFriendsInviteMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 15793;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForFriendsInviteMessage"/> class.
        /// </summary>
        public AskForFriendsInviteMessage()
        {
            // AskForFriendsInviteMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForFriendsInviteMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForFriendsInviteMessage(ByteStream Stream) : base(Stream)
        {
            // AskForFriendsInviteMessage.
        }
    }
}