namespace ClashRoyale.Messages.Server.Socials
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Logic.Player;

    public class OnlineInvitedFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20207;
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

        private readonly List<Player> Friends;
        private readonly List<Player> OnlineFriends;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineInvitedFriendsMessage"/> class.
        /// </summary>
        /// <param name="Friends">The friends.</param>
        public OnlineInvitedFriendsMessage(List<Player> Friends)
        {
            this.Friends       = Friends;
            this.OnlineFriends = new List<Player>(Friends.Count);

            foreach (var Friend in Friends)
            {
                if (Friend.IsConnected)
                {
                    this.OnlineFriends.Add(Friend);
                }
            }
        }

        /// <summary>
        /// Encodes this message.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.OnlineFriends.Count);
            this.Stream.WriteVInt(this.Friends.Count);

            foreach (var Friend in this.OnlineFriends)
            {
                this.Stream.WriteLong(Friend.PlayerId);
            }
        }
    }
}
 