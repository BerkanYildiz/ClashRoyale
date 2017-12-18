namespace ClashRoyale.Server.Network.Packets.Server
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Player;

    internal class OnlineInvitedFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20207;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        /// <param name="Device">The device.</param>
        /// <param name="Friends">The friends.</param>
        public OnlineInvitedFriendsMessage(Device Device, List<Player> Friends) : base(Device)
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
        internal override void Encode()
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
 