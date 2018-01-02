namespace ClashRoyale.Server.Network.Packets.Server
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages;

    internal class OnlineFacebookFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20208;
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
        /// Initializes a new instance of the <see cref="OnlineFacebookFriendsMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Friends">The friends.</param>
        public OnlineFacebookFriendsMessage(Device Device, List<Player> Friends) : base(Device)
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

            Logging.Warning(this.GetType(), "OnlineFriends : " + this.OnlineFriends.Count + ".");
            Logging.Warning(this.GetType(), "Friends : " + this.Friends.Count + ".");
        }

        /// <summary>
        /// Encodes this message.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.OnlineFriends.Count);
            this.Stream.WriteVInt(this.Friends.Count + 1);

            foreach (var Friend in this.OnlineFriends)
            {
                this.Stream.WriteLong(Friend.PlayerId);
            }
        }
    }
}
 