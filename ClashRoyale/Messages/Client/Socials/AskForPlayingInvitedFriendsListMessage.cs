namespace ClashRoyale.Messages.Client.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForPlayingInvitedFriendsListMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10504;
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
        /// Initializes a new instance of the <see cref="AskForPlayingInvitedFriendsListMessage"/> class.
        /// </summary>
        public AskForPlayingInvitedFriendsListMessage()
        {
            // AskForPlayingInvitedFriendsListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForPlayingInvitedFriendsListMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForPlayingInvitedFriendsListMessage(ByteStream Stream) : base(Stream)
        {
            // AskForPlayingInvitedFriendsListMessage.
        }
    }
}