namespace ClashRoyale.Messages.Client.Socials
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages.Server.Socials;

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
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForPlayingInvitedFriendsListMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForPlayingInvitedFriendsListMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            this.Device.NetworkManager.SendMessage(new FriendsListMessage(this.Device, new List<Player>(0)));
        }
    }
}