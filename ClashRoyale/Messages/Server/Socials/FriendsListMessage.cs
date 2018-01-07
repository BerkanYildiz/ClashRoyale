namespace ClashRoyale.Messages.Server.Socials
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Player;

    public class FriendsListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20105;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendsListMessage"/> class.
        /// </summary>
        /// <param name="Friends">The friends identifiers.</param>
        public FriendsListMessage(List<Player> Friends)
        {
            this.Friends = Friends;
        }

        /// <summary>
        /// Encodes this message.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteInt(1); // 0 = Invited | 1 = Facebook (?) | 2 = Gamecenter (?)
            this.Stream.WriteInt(this.Friends.Count);

            foreach (Player Friend in this.Friends)
            {
                Logging.Info(this.GetType(), "Friend : " + Friend.Name + ".");

                this.Stream.WriteLong(Friend.PlayerId);

                this.Stream.WriteBoolean(true);

                this.Stream.WriteLong(Friend.PlayerId);

                this.Stream.WriteString(Friend.Name);
                this.Stream.WriteString(Friend.ApiManager.Facebook.Identifier);
                this.Stream.WriteString(Friend.ApiManager.Gamecenter.Identifier);

                this.Stream.WriteVInt(Friend.ExpLevel);
                this.Stream.WriteVInt(Friend.Score);

                this.Stream.WriteBoolean(false); // Clan.

                // this.Stream.WriteString(null);
                // this.Stream.WriteString(null);

                // this.Stream.WriteVInt(1);
                
                this.Stream.EncodeData(Friend.Arena);

                this.Stream.WriteString(null);
                this.Stream.WriteString(null);
                
                this.Stream.WriteVInt(-1);

                this.Stream.WriteInt(0);
                this.Stream.WriteInt(0);
            }
        }
    }
}
 