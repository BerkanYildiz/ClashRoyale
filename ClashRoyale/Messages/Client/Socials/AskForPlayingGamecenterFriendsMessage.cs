namespace ClashRoyale.Messages.Client.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class AskForPlayingGamecenterFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10512;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Friend;
            }
        }

        public string[] FriendsIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForPlayingGamecenterFriendsMessage"/> class.
        /// </summary>
        public AskForPlayingGamecenterFriendsMessage()
        {
            // AskForPlayingGamecenterFriendsMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForPlayingGamecenterFriendsMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForPlayingGamecenterFriendsMessage(ByteStream Stream) : base(Stream)
        {
            // AskForPlayingGamecenterFriendsMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            int Count       = this.Stream.ReadVInt();

            this.FriendsIds = new string[Count];

            for (int I = 0; I < Count; I++)
            {
                this.FriendsIds[I] = this.Stream.ReadString();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.FriendsIds.Length);

            foreach (string FriendId in this.FriendsIds)
            {
                this.Stream.WriteString(FriendId);
            }
        }
    }
}