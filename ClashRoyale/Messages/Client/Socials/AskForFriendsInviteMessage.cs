namespace ClashRoyale.Messages.Client.Socials
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages.Server.Socials;

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
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForFriendsInviteMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForFriendsInviteMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            this.Device.NetworkManager.SendMessage(new FriendsInviteDataMessage(this.Device, this.Device.GameMode.Player.ToString()));
        }
    }
}